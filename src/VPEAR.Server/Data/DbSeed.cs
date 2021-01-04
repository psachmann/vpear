// <copyright file="DbSeed.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using VPEAR.Core;
using VPEAR.Core.Extensions;
using VPEAR.Core.Models;

namespace VPEAR.Server.Data
{
    internal static class DbSeed
    {
        private static IList<Device> devices = new List<Device>();
        private static IList<Filter> filters = new List<Filter>();
        private static IList<Firmware> firmwares = new List<Firmware>();
        private static IList<Frame> frames = new List<Frame>();
        private static IList<Sensor> sensors = new List<Sensor>();
        private static IList<Wifi> wifis = new List<Wifi>();

        static DbSeed()
        {
            devices.Clear();
            filters.Clear();
            firmwares.Clear();
            frames.Clear();
            sensors.Clear();
            wifis.Clear();

            foreach (var i in Enumerable.Range(1, 9))
            {
                var device = new Device()
                {
                    Address = $"address_{i}",
                    Class = $"class_{i}",
                    Id = new Guid($"00000000-0000-0000-0000-00000000000{i}"),
                    Name = $"name_{i}",
                    RequiredSensors = (uint)i,
                    Frequency = (uint)i,
                    Status = (DeviceStatus)((i - 1) % 4),
                };

                var filter = new Filter()
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
                    Ssid = $"ssid_{i}",
                };

                devices.Add(device);
                filters.Add(filter);
                firmwares.Add(firmware);
                frames.Add(frame);
                sensors.Add(sensor);
                wifis.Add(wifi);
            }
        }

        public static IList<Device> Devices
        {
            get { return devices.Clone()!; }
        }

        public static IList<Filter> Filters
        {
            get { return filters.Clone()!; }
        }

        public static IList<Firmware> Firmwares
        {
            get { return firmwares.Clone()!; }
        }

        public static IList<Frame> Frames
        {
            get { return frames.Clone()!; }
        }

        public static IList<Sensor> Sensors
        {
            get { return sensors.Clone()!; }
        }

        public static IList<Wifi> Wifis
        {
            get { return wifis.Clone()!; }
        }
    }
}
