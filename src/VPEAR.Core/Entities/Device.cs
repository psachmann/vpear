// <copyright file="Device.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Events;

namespace VPEAR.Core.Entities
{
    /// <summary>
    /// Db data model for entity framework.
    /// </summary>
    public class Device : AbstractEntity<Guid>
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The device IP address.</value>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The device class.</value>
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>A name for better recognition.</value>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The device name.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the frames object.
        /// </summary>
        /// <value>A list of frames.</value>
        public virtual IList<Frame> Frames { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The scanning frequency.</value>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the required sensors.
        /// </summary>
        /// <value>The number of required sensors.</value>
        public int RequiredSensors { get; set; }

        /// <summary>
        /// Gets or sets the filters object.
        /// </summary>
        /// <value>The filter details.</value>
        public virtual Filter Filter { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The device status.</value>
        public DeviceStatus Status { get; set; } = DeviceStatus.Stopped;

        /// <summary>
        /// Raises a new frequency changed event.
        /// </summary>
        /// <param name="newFrequency">The new device scanning frequency.</param>
        public void FrequencyChanged(int? newFrequency)
        {
            if (newFrequency == null || this.Frequency == newFrequency)
            {
                return;
            }

            this.Frequency = newFrequency.Value;
            this.Events.Add(new DeviceFrequencyChangedEvent(this));
        }

        /// <summary>
        /// Raises a new device status changed event.
        /// </summary>
        /// <param name="newStatus">The new device status.</param>
        public void StatusChanged(DeviceStatus? newStatus)
        {
            if (newStatus == null || this.Status == newStatus)
            {
                return;
            }

            this.Status = newStatus.Value;
            this.Events.Add(new DeviceStatusChangedEvent(this));
        }
    }
}
