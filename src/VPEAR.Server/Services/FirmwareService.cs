// <copyright file="FirmwareService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class FirmwareService : IFirmwareService
    {
        /// <inheritdoc/>
        public Task<Response> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PutAsync(Guid id, PutFirmwareRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
