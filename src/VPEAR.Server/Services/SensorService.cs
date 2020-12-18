// <copyright file="SensorService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
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
        public async Task<Response> GetFramesAsync(Guid id, uint? start, uint? stop)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var payload = new GetFramesResponse();
            var frames = await this.frames.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .ToListAsync();

            if (frames == null || frames.Count == 0)
            {
                statusCode = HttpStatusCode.NotFound;
                payload = null;
            }
            else if (start == null && stop == null)
            {
                statusCode = HttpStatusCode.OK;
                frames.ForEach(f =>
                {
                    // TODO: modify response
                    payload.Frames.Add(new FrameResponse()
                    {
                        Id = f.Index,
                        Readings = f.Readings,
                        Time = f.Time,
                    });
                });
            }
            else
            {
                return new Response(HttpStatusCode.InternalServerError);
            }

            return new Response(statusCode, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> GetSensorsAsync(Guid id)
        {
            var sensors = await this.sensors.Get()
                .Where(s => s.DeviceForeignKey.Equals(id))
                .ToListAsync();

            if (sensors != null && sensors.Count != 0)
            {
                var payload = new GetSensorsResponse();

                sensors.ForEach(s =>
                {
                    payload.Sensors.Add(new SensorResponse()
                    {
                        Columns = s.Columns,
                        Height = s.Height,
                        Maximum = s.Maximum,
                        Minimum = s.Minimum,
                        Name = s.Name,
                        Rows = s.Rows,
                        Units = s.Units,
                        Width = s.Width,
                    });
                });

                return new Response(HttpStatusCode.OK, payload);
            }
            else
            {
                return new Response(HttpStatusCode.NotFound);
            }
        }
    }
}
