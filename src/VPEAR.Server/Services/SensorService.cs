// <copyright file="SensorService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
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
        private readonly IRepository<Frame, Guid> frames;
        private readonly IRepository<Sensor, Guid> sensors;
        private readonly ILogger<SensorController> logger;

        public SensorService(
            IRepository<Frame, Guid> frames,
            IRepository<Sensor, Guid> sensors,
            ILogger<SensorController> logger)
        {
            this.frames = frames;
            this.sensors = sensors;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public Result<Container<GetFrameResponse>> GetFrames(Guid id, int start, int stop)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var frames = this.frames.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .OrderBy(f => f.CreatedAt)
                .ToList();

            if (frames == null || frames.Count == 0)
            {
                status = HttpStatusCode.NotFound;
                message = ErrorMessages.FramesNotFound;
            }
            else if (start == 0 && stop == 0)
            {
                var payload = new Container<GetFrameResponse>();

                frames.ForEach(f =>
                {
                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = f.Readings,
                        Time = f.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss.ff"),
                    });
                });

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.OK, payload);
            }
            else if (start < stop && stop < frames.Count)
            {
                var payload = new Container<GetFrameResponse>();

                frames.GetRange(start, stop)
                    .ForEach(f =>
                    {
                        payload.Items.Add(new GetFrameResponse()
                        {
                            Readings = f.Readings,
                            Time = f.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss.ff"),
                        });
                    });

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.OK, payload);
            }
            else if (start < stop && stop >= frames.Count)
            {
                var payload = new Container<GetFrameResponse>();

                frames.GetRange(start, frames.Count - 1)
                    .ForEach(f =>
                    {
                        payload.Items.Add(new GetFrameResponse()
                        {
                            Readings = f.Readings,
                            Time = f.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss.ff"),
                        });
                    });

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.PartialContent, payload);
            }
            else
            {
                // start is grater or equals stop
                status = HttpStatusCode.BadRequest;
                message = ErrorMessages.BadRequest;
            }

            return new Result<Container<GetFrameResponse>>(status, message);
        }

        /// <inheritdoc/>
        public Result<Container<GetSensorResponse>> GetSensors(Guid id)
        {
            var sensors = this.sensors.Get()
                .Where(s => s.DeviceForeignKey.Equals(id))
                .ToList();

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
