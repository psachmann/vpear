// <copyright file="DeviceService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IRepository<Device, Guid> devices;

        public DeviceService(ILogger<DeviceController> logger, IRepository<Device, Guid> devices)
        {
            this.logger = logger;
            this.devices = devices;
        }

        /// <inheritdoc/>
        public Task<Response> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PutAsync(Guid id, PutDeviceRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PostAsync(PostDeviceRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
