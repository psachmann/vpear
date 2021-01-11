// <copyright file="IVPEARClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    public interface IVPEARClient : IDisposable
    {
        Exception Error { get; }

        HttpResponseMessage Response { get; }

        Task<bool> CanConnectAsync();

        Task<bool> DeleteDeviceAsync(string deviceId);

        Task<Container<GetDeviceResponse>> GetDevicesAsync(DeviceStatus? status = null);

        Task<bool> PostDevicesAsync(string deviceId, string address, string subnetMask);

        Task<bool> PutDeviceAsync(string deviceId, string diplayName, int? frquency, int? requiredSesnors);

        Task<GetFiltersResponse> GetFiltersAsync(string deviceId);

        Task<bool> PutFiltersAsync(string deviceId, bool? spot, bool? smooth, bool? noise);

        Task<GetFirmwareResponse> GetFirmwareAsync(string deviceId);

        Task<bool> PutFirmwareAsync(string deviceId, string source, string upgrade, bool package = false);

        Task<GetPowerResponse> GetPowerAsync(string deviceId);

        Task<Container<GetFrameResponse>> GetFramesAsync(string deviceId, int? start, int? count);

        Task<Container<GetSensorResponse>> GetSensorsAsync(string deviceId);

        Task<bool> DeleteUserAsync(string name);

        Task<Container<GetUserResponse>> GetUsersAsync(string role = null);

        Task<bool> PutUserAsync(string name, string oldPassword = default, string newPassword = default, bool isVerified = false);

        Task<bool> LoginAsync(string name, string password);

        void Logout();

        Task<bool> RegisterAsync(string name, string password, bool isAdmin = false);

        Task<GetWifiResponse> GetWifiAsync(string deviceId);

        Task<bool> PutWifiAsync(string deviceId, string ssid, string password, string mode = null);
    }
}
