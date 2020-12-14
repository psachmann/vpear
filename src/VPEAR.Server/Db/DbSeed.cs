// <copyright file="DbSeed.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using VPEAR.Core;
using VPEAR.Core.Models;

namespace VPEAR.Server.Db
{
    public static class DbSeed
    {
        static DbSeed()
        {
            Devices = new List<Device>();
            Filters = new List<Filters>();
            Firmwares = new List<Firmware>();
            Frames = new List<Frame>();
            Sensors = new List<Sensor>();
            Wifis = new List<Wifi>();

            foreach (var i in Enumerable.Range(1, 4))
            {
                var device = new Device()
                {
                    Address = $"address_{i}",
                    Class = $"class_{i}",
                    Id = new Guid($"00000000-0000-0000-0000-00000000000{i}"),
                    Name = $"name_{i}",
                    RequiredSensors = (uint)i,
                    SampleFrequency = (uint)i,
                    Status = (DeviceStatus)((i - 1) % 4),
                };

                var filters = new Filters()
                {
                    DeviceForeignKey = device.Id,
                    Id = device.Id,
                };

                var firmware = new Firmware()
                {
                    DeviceForeignKey = device.Id,
                    Id = device.Id,
                    Source = $"source_{i}",
                    Upgrade = $"upgrade_{i}",
                    Version = $"version_{i}",
                };

                var frame = new Frame()
                {
                    DeviceForeignKey = device.Id,
                    Id = device.Id,
                    Index = i,
                    Time = $"time_{i}",
                };

                var sensor = new Sensor()
                {
                    DeviceForeignKey = device.Id,
                    Id = device.Id,
                    Name = $"name_{i}",
                    Units = $"units_{i}",
                };

                var wifi = new Wifi()
                {
                    DeviceForeignKey = device.Id,
                    Id = device.Id,
                    Mode = $"mode_{i}",
                    Neighbors = { $"neighbors_{i}" },
                    Password = $"password_{i}",
                    Ssid = $"ssid_{i}",
                };

                Devices.Add(device);
                Filters.Add(filters);
                Firmwares.Add(firmware);
                Frames.Add(frame);
                Sensors.Add(sensor);
                Wifis.Add(wifi);
            }
        }

        public static IList<Device> Devices { get; }

        public static IList<Filters> Filters { get; }

        public static IList<Firmware> Firmwares { get; }

        public static IList<Frame> Frames { get; }

        public static IList<Sensor> Sensors { get; }

        public static IList<Wifi> Wifis { get; }
    }
}
