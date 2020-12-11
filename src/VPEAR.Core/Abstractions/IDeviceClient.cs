// <copyright file="IDeviceClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface IDeviceClient
    {
        delegate IDeviceClient Factory(string baseAddress);

        string BaseAddress { get; }

        Task<DeviceResponse?> GetDeviceAsync();

        Task<SensorResponse?> GetSensorsAsync();

        Task<IList<FrameResponse>?> GetFramesAsync(int? after = null);

        Task<int?> GetFrequencyAsync();

        Task PutFrequencyAsync(int frequency);

        Task<int?> GetRequiredSensorsAsync();

        Task PutRequiredSensorsAsync(int requiredSensors);

        Task<GetFiltersResponse?> GetFiltersAsync();

        Task PutFiltersAsync(bool spot, bool smooth, bool noise);

        Task<GetPowerResponse?> GetPowerAsync();

        Task<DateTimeOffset?> GetTimeAsync();

        Task<WifiResponse?> GetWifiAsync();

        Task PutWifiAsync(string ssid, string password, string? mode = null);

        Task<FirmwareResponse?> GetFirmwareAsync();

        Task PutFirmwareAsync(string? source, string? upgrade, bool package = false);

        Task<dynamic> GetSSEAsync();
    }
}
