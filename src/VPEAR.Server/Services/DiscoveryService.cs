// <copyright file="DiscoveryService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Extensions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;

namespace VPEAR.Server.Services
{
    public class DiscoveryService : IDiscoveryService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<DeviceController> logger;

        public DiscoveryService(
            IRepository<Device, Guid> devices,
            DeviceClient.Factory factory,
            ILogger<DeviceController> logger)
        {
            this.devices = devices;
            this.factory = factory;
            this.logger = logger;
        }

        public async Task SearchDevicesAsync(IPAddress address, IPAddress subnetMask)
        {
            var addresses = GetSearchRange(address, subnetMask);
            var devices = new DeviceResponse[addresses.Count];

            Parallel.For(0, addresses.Count, async index =>
            {
                var client = this.factory.Invoke(addresses[index].ToString());
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

        private async Task InitializeDevicesAsync(IEnumerable<DeviceResponse> devices)
        {
            foreach (var device in devices)
            {
                var client = this.factory.Invoke(device.Address);
                var response = await client.GetAsync();
                var newDevice = new Device
                {
                    // TODO: will ef core also create all other entities e.g. Filters, Firmware etc?
                    Address = response.Device.Address,
                    Class = response.Device.Class,
                    DisplayName = string.Empty,
                    Filter = new Filter()
                    {
                        Noise = response.Filters.Noise,
                        Smooth = response.Filters.Smooth,
                        Spot = response.Filters.Spot,
                    },
                    Firmware = new Firmware()
                    {
                        Source = response.Firmware.Source,
                        Upgrade = response.Firmware.Upgrade,
                        Version = response.Firmware.Version,
                    },
                    Name = response.Device.Name,
                    RequiredSensors = response.SensorsRequired,
                    Frequency = response.Frequency,
                    Status = DeviceStatus.None,
                    Sensors = new List<Sensor>(),
                    Wifi = new Wifi()
                    {
                        Mode = response.Wifi.Mode,
                        Neighbors = response.Wifi.Neighbors,
                        Ssid = response.Wifi.Ssid,
                    },
                };

                foreach (var sensor in response.Sensors)
                {
                    newDevice.Sensors.Add(new Sensor()
                    {
                        Columns = sensor.Columns,
                        Height = sensor.Height,
                        Maximum = sensor.Maximum,
                        Minimum = sensor.Minimum,
                        Name = sensor.Name,
                        Rows = sensor.Rows,
                        Units = sensor.Units,
                        Width = sensor.Width,
                    });
                }

                // TODO: update time on client
                // await client.SyncTimeAsync()
                await this.devices.CreateAsync(newDevice);
            }
        }
    }
}
