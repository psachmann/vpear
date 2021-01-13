// <copyright file="IDeviceService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// Searches for devices with the given status. If no status provided all devices will be returned.
        /// </summary>
        /// <param name="status">The specific device status.</param>
        /// <returns>The result, which contains the devices with the given state.</returns>
        Task<Result<Container<GetDeviceResponse>>> GetAsync(DeviceStatus? status);

        /// <summary>
        /// Updates the given device.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <param name="request">The request data.</param>
        /// <returns>The result, which indicates the success of the operation.</returns>
        Task<Result<Null>> PutAsync(Guid id, PutDeviceRequest request);

        /// <summary>
        /// Starts the device discovery serice an searches for devices.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns>The result, which indicates the success of the operation.</returns>
        Task<Result<Null>> PostAsync(PostDeviceRequest request);

        /// <summary>
        /// Deletes the given device.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>The result, which indicates the success of the operation.</returns>
        Task<Result<Null>> DeleteAsync(Guid id);
    }
}
