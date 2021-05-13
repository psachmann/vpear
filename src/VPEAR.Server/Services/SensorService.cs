// <copyright file="SensorService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="ISensorService"/> interface.
    /// </summary>
    public class SensorService : ISensorService
    {
        private const int MaxCount = 100;
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Frame, Guid> frames;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<SensorController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorService"/> class.
        /// </summary>
        /// <param name="devices">The device repository.</param>
        /// <param name="frames">The frame repository.</param>
        /// <param name="factory">The device client factory.</param>
        /// <param name="logger">The service logger.</param>
        public SensorService(
            IRepository<Device, Guid> devices,
            IRepository<Frame, Guid> frames,
            DeviceClient.Factory factory,
            ILogger<SensorController> logger)
        {
            this.devices = devices;
            this.frames = frames;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetFrameResponse>>> GetFramesAsync(Guid id, int start, int count)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<Container<GetFrameResponse>>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            this.logger.LogWarning("Frames.Count " + device.Frames.Count);

            // checking for invalid values
            if (start < 0 || count < 0)
            {
                return new Result<Container<GetFrameResponse>>(HttpStatusCode.BadRequest, ErrorMessages.BadRequest);
            }

            var statusCode = HttpStatusCode.BadRequest;

            // prevent out of range exceptions
            if (start + count <= device.Frames.Count)
            {
                statusCode = HttpStatusCode.OK;
            }
            else
            {
                statusCode = HttpStatusCode.PartialContent;
                count = device.Frames.Count - start;
            }

            var frames = device.Frames
                .Distinct()
                .OrderBy(frame => frame.CreatedAt)
                .ToList();
            var items = new List<GetFrameResponse>();

            foreach (var frame in frames.GetRange(start, count))
            {
                var response = new GetFrameResponse()
                {
                    Filter = new GetFiltersResponse()
                    {
                        Noise = frame.Filter.Noise,
                        Smooth = frame.Filter.Smooth,
                        Spot = frame.Filter.Spot,
                    },
                    Readings = frame.Readings.FromJsonString<IList<IList<int>>>(),
                    Time = frame.CreatedAt,
                };

                items.Add(response);
            }

            return new Result<Container<GetFrameResponse>>(statusCode, new Container<GetFrameResponse>(start, device.Frames.Count, items));
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetSensorResponse>>> GetSensorsAsync(Guid id)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<Container<GetSensorResponse>>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Result<Container<GetSensorResponse>>(HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                return new Result<Container<GetSensorResponse>>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }

            var client = this.factory.Invoke(device.Address);

            if (await client.CanConnectAsync())
            {
                var sensors = await client.GetSensorsAsync();
                var items = new List<GetSensorResponse>();

                foreach (var sensor in sensors)
                {
                    items.Add(new GetSensorResponse()
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

                return new Result<Container<GetSensorResponse>>(HttpStatusCode.OK, new Container<GetSensorResponse>(0, items));
            }
            else
            {
                device.StatusChanged(DeviceStatus.NotReachable);

                await this.devices.UpdateAsync(device);

                return new Result<Container<GetSensorResponse>>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }
        }
    }
}
