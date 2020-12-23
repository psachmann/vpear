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
        Task<Response> GetAsync(DeviceStatus status);

        Task<Response> PutAsync(Guid id, PutDeviceRequest request);

        Task<Response> PostAsync(PostDeviceRequest request);

        Task<Response> DeleteAsync(Guid id);
    }
}
