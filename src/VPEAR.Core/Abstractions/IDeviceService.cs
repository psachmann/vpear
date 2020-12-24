// <copyright file="IDeviceService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface IDeviceService
    {
        Task<Result<Container<GetDeviceResponse>>> GetAsync(DeviceStatus status);

        Task<Result<Null>> PutAsync(Guid id, PutDeviceRequest request);

        Task<Result<Null>> PostAsync(PostDeviceRequest request);

        Task<Result<Null>> DeleteAsync(Guid id);
    }
}
