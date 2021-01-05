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
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class SensorService : ISensorService
    {
        private readonly IRepository<Filter, Guid> filters;
        private readonly IRepository<Frame, Guid> frames;
        private readonly IRepository<Sensor, Guid> sensors;
        private readonly ILogger<SensorController> logger;

        public SensorService(
            IRepository<Filter, Guid> filters,
            IRepository<Frame, Guid> frames,
            IRepository<Sensor, Guid> sensors,
            ILogger<SensorController> logger)
        {
            this.filters = filters;
            this.frames = frames;
            this.sensors = sensors;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetFrameResponse>>> GetFramesAsync(Guid id, int start, int stop)
        {
            var frames = await this.frames.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .OrderBy(f => f.CreatedAt)
                .ToListAsync();

            if (frames.Count == 0)
            {
                return new Result<Container<GetFrameResponse>>(HttpStatusCode.NotFound, ErrorMessages.FramesNotFound);
            }

            // TODO: load the filter and put it in the response
            if (start == 0 && stop == 0)
            {
                var payload = new Container<GetFrameResponse>();

                foreach (var frame in frames)
                {
                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = frame.Readings,
                        Time = frame.CreatedAt,
                    });
                }

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.OK, payload);
            }

            if (start < stop && stop < frames.Count)
            {
                var payload = new Container<GetFrameResponse>();

                foreach (var frame in frames.GetRange(start, stop - start))
                {
                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = frame.Readings,
                        Time = frame.CreatedAt,
                    });
                }

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.OK, payload);
            }

            if (start < stop && stop >= frames.Count)
            {
                var payload = new Container<GetFrameResponse>();

                foreach (var frame in frames.GetRange(start, frames.Count - start))
                {
                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = frame.Readings,
                        Time = frame.CreatedAt,
                    });
                }

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.PartialContent, payload);
            }

            return new Result<Container<GetFrameResponse>>(HttpStatusCode.BadRequest, ErrorMessages.BadRequest);
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetSensorResponse>>> GetSensorsAsync(Guid id)
        {
            var sensors = await this.sensors.Get()
                .Where(s => s.DeviceForeignKey.Equals(id))
                .ToListAsync();

            if (sensors == null || sensors.Count == 0)
            {
                return new Result<Container<GetSensorResponse>>(HttpStatusCode.NotFound, ErrorMessages.SensorsNotFound);
            }
            else
            {
                var payload = new Container<GetSensorResponse>();

                sensors.ForEach(s =>
                {
                    payload.Items.Add(new GetSensorResponse()
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

                return new Result<Container<GetSensorResponse>>(HttpStatusCode.OK, payload);
            }
        }
    }
}
