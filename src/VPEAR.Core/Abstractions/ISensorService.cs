// <copyright file="ISensorService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface ISensorService
    {
        /// <summary>
        /// Gets the connected sensors for the given device.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>The result, which contains the connected sensors.</returns>
        Task<Result<Container<GetSensorResponse>>> GetSensorsAsync(Guid id);

        /// <summary>
        /// Gets all frames from the start index plus count.
        /// If start and count 0, then all frames will be return.
        /// If start plus count bigger than the actual frames count, then all frames from start until the end will be returned.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <param name="start">The start index.</param>
        /// <param name="count">The frames count.</param>
        /// <returns>The result, witch contains the frames.</returns>
        Task<Result<Container<GetFrameResponse>>> GetFramesAsync(Guid id, int start, int count);
    }
}
