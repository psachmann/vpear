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
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface IVPEARClient : IDisposable
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error the occurred.</value>
        Exception Error { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message from the exception or http response.</value>
        string ErrorMessage { get; }

        /// <summary>
        /// Indicates if the device is reachable or not.
        /// </summary>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> CanConnectAsync();

        /// <summary>
        /// Deletes the given device.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> DeleteDeviceAsync(string deviceId);

        /// <summary>
        /// Gets all devices with the given status or all, if no status is provided.
        /// </summary>
        /// <param name="status">The dive status.</param>
        /// <returns>A container, which contains the found devices or null if an error occurred.</returns>
        Task<Container<GetDeviceResponse>> GetDevicesAsync(DeviceStatus? status = default);

        /// <summary>
        /// Starts the device discovery on the server.
        /// </summary>
        /// <param name="address">An address from the subnet.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PostDevicesAsync(string address, string subnetMask);

        /// <summary>
        /// Updates device information on the server.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="displayName">The device display name.</param>
        /// <param name="frequency">The device frequency.</param>
        /// <param name="requiredSensors">The number of required sensors.</param>
        /// <param name="status">The device status.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutDeviceAsync(string deviceId, string displayName = default, int? frequency = default, int? requiredSensors = default, DeviceStatus? status = default);

        /// <summary>
        /// Gets the filters from the device.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>The device filters or null if an error occurred.</returns>
        Task<GetFiltersResponse> GetFiltersAsync(string deviceId);

        /// <summary>
        /// Updates the device filters.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="spot">If spot filter should be applied.</param>
        /// <param name="smooth">If smooth filter should be applied.</param>
        /// <param name="noise">If the noise filter should be applied.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutFiltersAsync(string deviceId, bool? spot, bool? smooth, bool? noise);

        /// <summary>
        /// Gets the device firmware.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>The device firmware or null if an error occurred.</returns>
        Task<GetFirmwareResponse> GetFirmwareAsync(string deviceId);

        /// <summary>
        /// Updates the firmware data on the devices. Can only be used by an admin.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="source">The firmware channel (stable or unstable).</param>
        /// <param name="upgrade">The upgrade step (unknown or next).</param>
        /// <param name="package">Indicates if the upgrade process should be started.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutFirmwareAsync(string deviceId, string source, string upgrade, bool package = false);

        /// <summary>
        /// Gets the device power information.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>The device power information or null if an error occurred.</returns>
        Task<GetPowerResponse> GetPowerAsync(string deviceId);

        /// <summary>
        /// Gets the recorded frames from a device.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="start">The start index.</param>
        /// <param name="count">The frame count.</param>
        /// <returns>A container, which contains the found frames or null if an error occurred.</returns>
        Task<Container<GetFrameResponse>> GetFramesAsync(string deviceId, int? start, int? count);

        /// <summary>
        /// Gets the connected sensors from a device.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>A container, which contains the connected sensor or null if an error occurred.</returns>
        Task<Container<GetSensorResponse>> GetSensorsAsync(string deviceId);

        /// <summary>
        /// Deletes the given user. Can only be used by an admin.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> DeleteUserAsync(string name);

        /// <summary>
        /// Gets users with the given role or all users if no role is provided. Can only be used by an admin.
        /// </summary>
        /// <param name="role">The user role.</param>
        /// <returns>A container, which contains the found users or null if an error occurred.</returns>
        Task<Container<GetUserResponse>> GetUsersAsync(string role = null);

        /// <summary>
        /// Updates the user information. Can only be used by an admin.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <param name="oldPassword">The old user password.</param>
        /// <param name="newPassword">The new user password.</param>
        /// <param name="isVerified">Indicates if the user is verified or not.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutUserAsync(string name, string oldPassword = default, string newPassword = default, bool isVerified = false);

        /// <summary>
        /// Logs the user in and acquires an access token.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> LoginAsync(string name, string password);

        /// <summary>
        /// Logs the user out.
        /// </summary>
        void Logout();

        /// <summary>
        /// Registers a user, but the admin has to verify the user, before the new user can use login.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <param name="password">The user password.</param>
        /// <param name="isAdmin">Indicates if the user is an admin.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> RegisterAsync(string name, string password, bool isAdmin = false);

        /// <summary>
        /// Gets the device wifi information.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>The device wifi information or null if an error occurred.</returns>
        Task<GetWifiResponse> GetWifiAsync(string deviceId);

        /// <summary>
        /// Updates the device wifi information.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="ssid">The ssid for the network to connect to.</param>
        /// <param name="password">The password for the network.</param>
        /// <param name="mode">The device wifi mode.</param>
        /// <returns>A boolean, which indicates the success of the operation.</returns>
        Task<bool> PutWifiAsync(string deviceId, string ssid, string password, string mode = default);
    }
}
