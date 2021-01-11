// <copyright file="DeviceStatusChangedEvent.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;

namespace VPEAR.Core.Events
{
    /// <summary>
    /// This class represents a device status changed event.
    /// </summary>
    public class DeviceStatusChangedEvent : AbstractEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStatusChangedEvent"/> class.
        /// </summary>
        /// <param name="device">The original device state.</param>
        public DeviceStatusChangedEvent(Device device)
        {
            this.OriginalValue = device;
        }

        /// <summary>
        /// Gets the original device state.
        /// </summary>
        /// <value>The original device state.</value>
        public Device OriginalValue { get; }
    }
}
