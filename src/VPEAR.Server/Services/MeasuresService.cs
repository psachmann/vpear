// <copyright file="MeasuresService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class MeasuresService : IMeasuresService
    {
        /// <inheritdoc/>
        public Task<Response> GetFramesAsync(Guid id, int? start, int? stop)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> GetSensorsAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
