// <copyright file="SensorService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Controllers;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class SensorService : ISensorService
    {
        private readonly ILogger<SensorController> logger;
        private readonly IRepository<Frame, Guid> frames;
        private readonly IRepository<Sensor, Guid> sensors;

        public SensorService(
            ILogger<SensorController> logger,
            IRepository<Frame, Guid> frames,
            IRepository<Sensor, Guid> sensors)
        {
            this.logger = logger;
            this.frames = frames;
            this.sensors = sensors;
        }

        /// <inheritdoc/>
        public Task<Response> GetFramesAsync(Guid id, uint? start, uint? stop)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> GetSensorsAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
