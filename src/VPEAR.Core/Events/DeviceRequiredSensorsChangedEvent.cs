// <copyright file="DeviceRequiredSensorsChangedEvent.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;

namespace VPEAR.Core.Events
{
    /// <summary>
    /// This class represents a device required sensors changed event.
    /// </summary>
    public class DeviceRequiredSensorsChangedEvent : AbstractEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceRequiredSensorsChangedEvent"/> class.
        /// </summary>
        /// <param name="device">The original device state.</param>
        /// <param name="newRequiredSensors">The new required sensors amount.</param>
        public DeviceRequiredSensorsChangedEvent(Device device, int newRequiredSensors)
        {
            this.OriginalValue = device;
            this.NewValue = newRequiredSensors;
        }

        /// <summary>
        /// Gets the original device state.
        /// </summary>
        /// <value>The original device state.</value>
        public Device OriginalValue { get; }

        /// <summary>
        /// Gets the new required sensors value.
        /// </summary>
        /// <value>The new amount of required sensors.</value>
        public int NewValue { get; }
    }
}
