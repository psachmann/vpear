// <copyright file="AbstractEvent.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using MediatR;
using System;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Represents the base class for all events.
    /// </summary>
    public abstract class AbstractEvent : INotification
    {
        /// <summary>
        /// Gets or sets the occurred date.
        /// </summary>
        /// <value>The date and time when the event occurred.</value>
        public DateTimeOffset Occurred { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
