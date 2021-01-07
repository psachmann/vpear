// <copyright file="IDeviceClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
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
        Task<bool> CanConnectAsync();

        Task<ApiResponse> GetAsync();

        Task<DeviceResponse> GetDeviceAsync();

        Task<IList<SensorResponse>> GetSensorsAsync();

        Task<IList<FrameResponse>> GetFramesAsync(int? after = null);

        Task<int?> GetFrequencyAsync();

        Task<bool> PutFrequencyAsync(int? frequency);

        Task<int?> GetRequiredSensorsAsync();

        Task<bool> PutRequiredSensorsAsync(int? requiredSensors);

        Task<FiltersResponse> GetFiltersAsync();

        Task<bool> PutFiltersAsync(bool? spot, bool? smooth, bool? noise);

        Task<PowerResponse> GetPowerAsync();

        Task<DateTimeOffset?> GetTimeAsync();

        Task<bool> PutTimeAsync(DateTimeOffset time);

        Task<WifiResponse> GetWifiAsync();

        Task<bool> PutWifiAsync(string ssid, string password = null, string mode = null);

        Task<FirmwareResponse> GetFirmwareAsync();

        Task<bool> PutFirmwareAsync(string source = null, string upgrade = null, bool package = false);
    }
}
