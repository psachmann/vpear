// <copyright file="IPowerService.cs" company="Patrick Sachmann">
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
    public interface IPowerService
    {
        /// <summary>
        /// Gets the device power information.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>Http status code and device power information.</returns>
        Task<Result<GetPowerResponse>> GetAsync(Guid id);
    }
}
