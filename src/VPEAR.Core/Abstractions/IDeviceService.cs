// <copyright file="IDeviceService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    public interface IDeviceService
    {
        Task<Response> GetAsync(Guid id);

        Task<Response> PutAsync(Guid id, PutDeviceRequest request);

        Task<Response> PostAsync(PostDeviceRequest request);

        Task<Response> DeleteAsync(Guid id);
    }
}
