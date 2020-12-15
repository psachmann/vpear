// <copyright file="FirmwareService.cs" company="Patrick Sachmann">
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
    public class FirmwareService : IFirmwareService
    {
        private readonly ILogger<FirmwareController> logger;
        private readonly IRepository<Device, Guid> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirmwareService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="repository">The device repository for database access.</param>
        public FirmwareService(ILogger<FirmwareController> logger, IRepository<Device, Guid> repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<Response> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PutAsync(Guid id, PutFirmwareRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
