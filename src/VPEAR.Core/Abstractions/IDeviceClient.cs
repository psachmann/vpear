// <copyright file="IDeviceClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Entities;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface IDeviceClient : IDisposable
    {
        /// <summary>
        /// Indicates if the device is reachable or not.
        /// </summary>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> CanConnectAsync();

        /// <summary>
        /// Gets the complete device information.
        /// </summary>
        /// <returns>The complete device information.</returns>
        Task<ApiResponse> GetAsync();

        /// <summary>
        /// Gets the device address, class and name.
        /// </summary>
        /// <returns>The device address, class and name.</returns>
        Task<DeviceResponse> GetDeviceAsync();

        /// <summary>
        /// Gets the list of connected sensors.
        /// </summary>
        /// <returns>A list of connected sensors.</returns>
        Task<IList<SensorResponse>> GetSensorsAsync();

        /// <summary>
        /// Gets the recorded frames.
        /// </summary>
        /// <param name="after">The frame id, to get all frames after that specific frame.</param>
        /// <returns>A list of recorded frames.</returns>
        Task<IList<FrameResponse>> GetFramesAsync(int? after = null);

        /// <summary>
        /// Gets the device scanning frequency.
        /// </summary>
        /// <returns>The device scanning frequency.</returns>
        Task<int?> GetFrequencyAsync();

        /// <summary>
        /// Updates the device scanning frequency.
        /// </summary>
        /// <param name="frequency">The new scanning frequency.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutFrequencyAsync(int? frequency);

        /// <summary>
        /// Gets the number of regired sensor.
        /// </summary>
        /// <returns>The number of required sensors.</returns>
        Task<int?> GetRequiredSensorsAsync();

        /// <summary>
        /// Updates the number of required sensors.
        /// </summary>
        /// <param name="requiredSensors">The new number of required sensors.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutRequiredSensorsAsync(int? requiredSensors);

        /// <summary>
        /// Gets the current device filters.
        /// </summary>
        /// <returns>The current device filters.</returns>
        Task<FiltersResponse> GetFiltersAsync();

        /// <summary>
        /// Updates the device filters.
        /// </summary>
        /// <param name="spot">If spot filter should be applied.</param>
        /// <param name="smooth">If smooth filter should be applied.</param>
        /// <param name="noise">If the noise filter should be applied.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutFiltersAsync(bool? spot, bool? smooth, bool? noise);

        /// <summary>
        /// Gets the device power information.
        /// </summary>
        /// <returns>The current device power information.</returns>
        Task<PowerResponse> GetPowerAsync();

        /// <summary>
        /// Gets the current device time.
        /// </summary>
        /// <returns>The current device time.</returns>
        Task<DateTimeOffset?> GetTimeAsync();

        /// <summary>
        /// Updates the device time.
        /// </summary>
        /// <param name="time">The new device time.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutTimeAsync(DateTimeOffset time);

        /// <summary>
        /// Gets the device wifi information.
        /// </summary>
        /// <returns>The device wifi information.</returns>
        Task<WifiResponse> GetWifiAsync();

        /// <summary>
        /// Updates the device wifi information.
        /// </summary>
        /// <param name="ssid">The ssid for the network to connect to.</param>
        /// <param name="password">The password for the network.</param>
        /// <param name="mode">The device wifi mode.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutWifiAsync(string ssid, string password = null, string mode = null);

        /// <summary>
        /// Gets the firmware from the device or null if not reachable.
        /// </summary>
        /// <returns>The device firmware.</returns>
        Task<FirmwareResponse> GetFirmwareAsync();

        /// <summary>
        /// Updates the firmware data on the devices.
        /// </summary>
        /// <param name="source">The firmware channel (stable or unstable).</param>
        /// <param name="upgrade">The upgrade step (unknown or next).</param>
        /// <param name="package">Indicates if the upgrade process should be started.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutFirmwareAsync(string source = null, string upgrade = null, bool package = false);

        /// <summary>
        /// Syncs the server state with the device.
        /// </summary>
        /// <param name="device">The server device entity.</param>
        /// <param name="devices">The server device repository to load all references.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> SyncAsync(Device device, IRepository<Device, Guid> devices);
    }
}
