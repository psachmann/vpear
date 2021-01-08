// <copyright file="VPEARDbContextFixture.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using VPEAR.Core;
using VPEAR.Core.Models;
using VPEAR.Server.Data;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class VPEARDbContextFixture : IDisposable
    {
        private static readonly object Lock = new object();

        public VPEARDbContextFixture()
        {
            Seed();

            this.Context = new VPEARDbContext(GetDbContextOptions());
        }

        public VPEARDbContext Context { get; private set; }

        public void Dispose()
        {
            this.Context.Dispose();
            GC.SuppressFinalize(this);
        }

        private static DbContextOptions<VPEARDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<VPEARDbContext>()
                .UseInMemoryDatabase(Schemas.DbSchema);

            return builder.Options;
        }

        private static void Seed()
        {
            lock (Lock)
            {
                using var context = new VPEARDbContext(GetDbContextOptions());
                var devices = new List<Device>();
                var states = new List<(Guid Id, DeviceStatus Status)>()
                {
                    Mocks.Archived,
                    Mocks.NotReachable,
                    Mocks.Recording,
                    Mocks.Stopped,
                };

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                foreach (var (id, status) in states)
                {
                    var device = new Device()
                    {
                        Address = "http://192.168.178.8",
                        Class = "class",
                        DisplayName = "display_name",
                        Frames = new List<Frame>(),
                        Frequency = 60,
                        Id = id,
                        Name = "name",
                        RequiredSensors = 1,
                        Sensors = new List<Sensor>(),
                        Status = status,
                    };

                    var filter = new Filter()
                    {
                        Device = device,
                        DeviceForeignKey = id,
                        Id = id,
                        Frames = new List<Frame>(),
                        Noise = true,
                        Smooth = true,
                        Spot = true,
                    };

                    var firmware = new Firmware()
                    {
                        Device = device,
                        DeviceForeignKey = id,
                        Id = id,
                        Source = "source",
                        Upgrade = "upgrade",
                        Version = "version",
                    };

                    var frame = new Frame()
                    {
                        Device = device,
                        DeviceForeignKey = id,
                        Filter = filter,
                        Id = id,
                        Index = 1,

                        // TODO: generate more test values
                        Readings = new List<IList<int>>(),

                        // TODO: eventually remove time
                        Time = "time",
                    };

                    var sensor = new Sensor()
                    {
                        Columns = 1,
                        Device = device,
                        DeviceForeignKey = id,
                        Id = id,
                        Height = 1,
                        Maximum = 1,
                        Minimum = 1,
                        Name = "name",
                        Rows = 1,
                        Units = "units",
                        Width = 1,
                    };

                    var wifi = new Wifi()
                    {
                        Device = device,
                        DeviceForeignKey = id,
                        Id = id,
                        Mode = "mode",
                        Neighbors = new List<string>(),
                        Ssid = "ssid",
                    };

                    device.Filter = filter;
                    device.Firmware = firmware;
                    device.Frames.Add(frame);
                    device.Sensors.Add(sensor);
                    device.Wifi = wifi;
                    filter.Frames.Add(frame);

                    devices.Add(device);
                }

                context.AddRange(devices);
                context.SaveChanges();
            }
        }
    }
}
