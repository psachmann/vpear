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
        public async Task<Result<Container<GetFrameResponse>>> GetFramesAsync(Guid id, int start, int stop)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var frames = await this.frames.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .OrderBy(f => f.CreatedAt)
                .ToListAsync();

            if (frames == null || frames.Count == 0)
            {
                status = HttpStatusCode.NotFound;
                payload = null;
            }
            else if (start == 0 && stop == 0)
            {
                status = HttpStatusCode.OK;
                frames.ForEach(f =>
                {
                    payload.Frames.Add(new FrameResponse()
                    {
                        Readings = f.Readings,
                        Time = f.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss.ff"),
                    });
                });
            }
            else if (start < stop && stop < frames.Count)
            {
                status = HttpStatusCode.OK;
                frames.GetRange(start, stop)
                    .ForEach(f =>
                    {
                        payload.Frames.Add(new FrameResponse()
                        {
                            Readings = f.Readings,
                            Time = f.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss.ff"),
                        });
                    });
            }
            else if (start < stop && stop >= frames.Count)
            {
                status = HttpStatusCode.PartialContent;
                frames.GetRange(start, frames.Count - 1)
                    .ForEach(f =>
                    {
                        payload.Frames.Add(new FrameResponse()
                        {
                            Readings = f.Readings,
                            Time = f.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss.ff"),
                        });
                    });
            }
            else
            {
                // start is grater or equals stop
                status = HttpStatusCode.BadRequest;
                payload = null;
            }

            return new Result<Container<GetFrameResponse>>(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetSensorResponse>>> GetSensorsAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var sensors = await this.sensors.Get()
                .Where(s => s.DeviceForeignKey.Equals(id))
                .ToListAsync();

            if (sensors == null || sensors.Count == 0)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else
            {
                status = HttpStatusCode.OK;
                payload = new Container<GetSensorResponse>();

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
            }

            return new Result<Container<GetSensorResponse>>(status, payload);
        }
    }
}
