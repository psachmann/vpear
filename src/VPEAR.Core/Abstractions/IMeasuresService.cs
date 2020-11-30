// <copyright file="IMeasuresService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    public interface IMeasuresService
    {
        Task<Response> GetSensorsAsync(Guid id);

        Task<Response> GetFramesAsync(Guid id, int? start, int? stop);
    }
}
