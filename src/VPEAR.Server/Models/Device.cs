// <copyright file="Device.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using VPEAR.Core;
using VPEAR.Server.Db;

namespace VPEAR.Server.Models
{
    /// <summary>
    /// Db data model for entity framework.
    /// </summary>
    public class Device : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The device ip address.</value>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The device class.</value>
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The device name.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sensors object.
        /// </summary>
        /// <value>The sensor details.</value>
        public IList<Sensor> Sensors { get; set; } = new List<Sensor>();

        /// <summary>
        /// Gets or sets the frames object.
        /// </summary>
        /// <value>A list of frames.</value>
        public IList<Frame> Frames { get; set; } = new List<Frame>();

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The sample frequency.</value>
        public uint SampleFrequency { get; set; }

        /// <summary>
        /// Gets or sets the required sensors.
        /// </summary>
        /// <value>The number of required sensors.</value>
        public uint RequiredSensors { get; set; }

        /// <summary>
        /// Gets or sets the filters object.
        /// </summary>
        /// <value>The filter details.</value>
        public Filters Filters { get; set; } = new Filters();

        /// <summary>
        /// Gets or sets the power object.
        /// </summary>
        /// <value>The power details.</value>
        public Power Power { get; set; } = new Power();

        /// <summary>
        /// Gets or sets the wifi object.
        /// </summary>
        /// <value>The wifi details.</value>
        public Wifi Wifi { get; set; } = new Wifi();

        /// <summary>
        /// Gets or sets the firmware object.
        /// </summary>
        /// <value>The firmware details.</value>
        public Firmware Firmware { get; set; } = new Firmware();

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The device status.</value>
        public DeviceStatus Status { get; set; } = DeviceStatus.Stopped;
    }
}
