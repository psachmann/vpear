// <copyright file="DeviceFrequencyChangedEvent.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;

namespace VPEAR.Core.Events
{
    /// <summary>
    /// This class represents a device frequency changed event.
    /// </summary>
    public class DeviceFrequencyChangedEvent : AbstractEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFrequencyChangedEvent"/> class.
        /// </summary>
        /// <param name="device">The original state.</param>
        /// <param name="newFrequency">The new scanning frequency.</param>
        public DeviceFrequencyChangedEvent(Device device, int newFrequency)
        {
            this.OriginalValue = device;
            this.NewValue = newFrequency;
        }

        /// <summary>
        /// Gets the original device state.
        /// </summary>
        /// <value>The original device state.</value>
        public Device OriginalValue { get; }

        /// <summary>
        /// Gets the new scanning frequency.
        /// </summary>
        /// <value>The new device scanning frequency.</value>
        public int NewValue { get; }
    }
}
