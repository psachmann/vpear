// <copyright file="IWifiService.cs" company="Patrick Sachmann">
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
    public interface IWifiService
    {
        /// <summary>
        /// Gets the device wifi information.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>Http status code and device wifi information.</returns>
        Task<Response> GetAsync(Guid id);

        /// <summary>
        /// Updates the device wife informion.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code.</returns>
        Task<Response> PutAsync(Guid id, PutWifiRequest request);
    }
}
