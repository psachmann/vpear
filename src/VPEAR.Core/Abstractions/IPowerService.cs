// <copyright file="IPowerService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    public interface IPowerService
    {
        Task<Response> GetAsync(Guid id);
    }
}
