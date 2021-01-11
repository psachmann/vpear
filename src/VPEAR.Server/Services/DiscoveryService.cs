// <copyright file="DiscoveryService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDiscoveryService"/> interface.
    /// </summary>
    public class DiscoveryService : IDiscoveryService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<DeviceController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryService"/> class.
        /// </summary>
        /// <param name="devices">The device repository.</param>
        /// <param name="factory">The device client factory.</param>
        /// <param name="logger">The service logger.</param>
        public DiscoveryService(
            IRepository<Device, Guid> devices,
            DeviceClient.Factory factory,
            ILogger<DeviceController> logger)
        {
            this.devices = devices;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task SearchDevicesAsync(IPAddress address, IPAddress subnetMask)
        {
            var addresses = GetSearchRange(address, subnetMask);
            var devices = new DeviceResponse[addresses.Count];

            Parallel.For(0, addresses.Count, async index =>
            {
                var client = this.factory.Invoke($"http://{addresses[index]}");
                var response = await client.GetDeviceAsync();

                if (response != null)
                {
                    devices[index] = response;
                }
            });

            await this.InitializeDevicesAsync(devices.Where(device => device != null));
        }

        private static IList<IPAddress> GetSearchRange(IPAddress address, IPAddress subnetMask)
        {
            var addresses = new List<IPAddress>();
            var networkAddressBytes = address.GetNetworkAddress(subnetMask).GetAddressBytes();
            var broadcastAddressBytes = address.GetBroadcastAddress(subnetMask).GetAddressBytes();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(networkAddressBytes);
                Array.Reverse(broadcastAddressBytes);
            }

            var networkaddressUInt32 = BitConverter.ToUInt32(networkAddressBytes);
            var broadcastAddressUInt32 = BitConverter.ToUInt32(broadcastAddressBytes);

            for (var i = networkaddressUInt32 + 1; i < broadcastAddressUInt32; i++)
            {
                addresses.Add(IPAddress.Parse(i.ToString()));
            }

            return addresses;
        }

        private async Task CreateDeviceAsync(ApiResponse response, IDeviceClient client)
        {
            var newDevice = new Device
            {
                Address = response.Device.Address,
                Class = response.Device.Class,
                DisplayName = string.Empty,
                Filter = new Filter()
                {
                    Noise = response.Filters.Noise,
                    Smooth = response.Filters.Smooth,
                    Spot = response.Filters.Spot,
                },
                Name = response.Device.Name,
                RequiredSensors = response.SensorsRequired,
                Frequency = response.Frequency,
                Status = DeviceStatus.Stopped,
            };

            await this.devices.CreateAsync(newDevice);
            await client.PutTimeAsync(DateTimeOffset.UtcNow);
        }

        private async Task InitializeDevicesAsync(IEnumerable<DeviceResponse> deviceResponses)
        {
            foreach (var deviceResponse in deviceResponses)
            {
                var client = this.factory.Invoke(deviceResponse.Address);
                var response = await client.GetAsync();
                var knownDevice = await this.devices.Get()
                    .Where(device => device.Address == deviceResponse.Address && device.Status != DeviceStatus.Archived)
                    .FirstOrDefaultAsync();

                // we create a new device if the address is new or a device with the same address was archived
                if (knownDevice == null || knownDevice.Status == DeviceStatus.Archived)
                {
                    await this.CreateDeviceAsync(response, client);

                    continue;
                }

                // we will sync the old device and set it to stopped
                if (knownDevice.Status == DeviceStatus.NotReachable)
                {
                    await client.SyncAsync(knownDevice, this.devices);

                    knownDevice.StatusChanged(DeviceStatus.Stopped);
                    knownDevice.Status = DeviceStatus.Stopped;

                    await this.devices.UpdateAsync(knownDevice);

                    continue;
                }

                // we will do nothing with the already known devices in stopped and recording
            }
        }
    }
}
