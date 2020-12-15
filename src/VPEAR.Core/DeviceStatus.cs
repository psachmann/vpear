// <copyright file="DeviceStatus.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace VPEAR.Core
{
    /// <summary>
    /// The current device state.
    /// </summary>
    public enum DeviceStatus
    {
        /// <summary>
        /// The device has stopped recording data from a patient.
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// The device is recording data from a patient.
        /// </summary>
        Recording,

        /// <summary>
        /// The device doesn't exist anymore, but the patient is kept in the database.
        /// </summary>
        Archived,

        /// <summary>
        /// The device is currently not reachable (new address, out of energy).
        /// </summary>
        NotReachable,
    }
}
