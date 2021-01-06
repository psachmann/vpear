// <copyright file="SensorService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

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
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Frame, Guid> frames;
        private readonly ILogger<SensorController> logger;

        public SensorService(
            IRepository<Device, Guid> devices,
            IRepository<Frame, Guid> frames,
            ILogger<SensorController> logger)
        {
            this.devices = devices;
            this.frames = frames;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetFrameResponse>>> GetFramesAsync(Guid id, int start, int count)
        {
            var device = await this.devices.GetAsync(id);
            var payload = new Container<GetFrameResponse>();

            if (device == null)
            {
                return new Result<Container<GetFrameResponse>>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            await this.devices.GetCollectionAsync(device, device => device.Frames.OrderBy(frame => frame.CreatedAt));

            if (start == 0 && count == 0)
            {
                foreach (var frame in device.Frames)
                {
                    await this.frames.GetReferenceAsync(frame, frame => frame.Filter);

                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = frame.Readings,
                        Time = frame.CreatedAt,
                        Filter = new GetFiltersResponse()
                        {
                            Noise = frame.Filter.Noise,
                            Smooth = frame.Filter.Smooth,
                            Spot = frame.Filter.Spot,
                        },
                    });
                }

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.OK, payload);
            }

            if (start < count && count < device.Frames.Count)
            {
                foreach (var frame in device.Frames.ToList().GetRange(start, count))
                {
                    await this.frames.GetReferenceAsync(frame, frame => frame.Filter);

                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = frame.Readings,
                        Time = frame.CreatedAt,
                        Filter = new GetFiltersResponse()
                        {
                            Noise = frame.Filter.Noise,
                            Smooth = frame.Filter.Smooth,
                            Spot = frame.Filter.Spot,
                        },
                    });
                }

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.OK, payload);
            }

            if (start < count && count >= device.Frames.Count)
            {
                foreach (var frame in device.Frames.ToList().GetRange(start, device.Frames.Count - start))
                {
                    await this.frames.GetReferenceAsync(frame, frame => frame.Filter);

                    payload.Items.Add(new GetFrameResponse()
                    {
                        Readings = frame.Readings,
                        Time = frame.CreatedAt,
                        Filter = new GetFiltersResponse()
                        {
                            Noise = frame.Filter.Noise,
                            Smooth = frame.Filter.Smooth,
                            Spot = frame.Filter.Spot,
                        },
                    });
                }

                return new Result<Container<GetFrameResponse>>(HttpStatusCode.PartialContent, payload);
            }

            return new Result<Container<GetFrameResponse>>(HttpStatusCode.BadRequest, ErrorMessages.BadRequest);
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetSensorResponse>>> GetSensorsAsync(Guid id)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<Container<GetSensorResponse>>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }
            else
            {
                var payload = new Container<GetSensorResponse>();

                await this.devices.GetCollectionAsync(device, device => device.Sensors);

                foreach (var sensor in device.Sensors)
                {
                    payload.Items.Add(new GetSensorResponse()
                    {
                        Columns = sensor.Columns,
                        Height = sensor.Height,
                        Maximum = sensor.Maximum,
                        Minimum = sensor.Minimum,
                        Name = sensor.Name,
                        Rows = sensor.Rows,
                        Units = sensor.Units,
                        Width = sensor.Width,
                    });
                }

                return new Result<Container<GetSensorResponse>>(HttpStatusCode.OK, payload);
            }
        }
    }
}
