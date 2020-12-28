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
        Result<Container<GetSensorResponse>> GetSensors(Guid id);

        Result<Container<GetFrameResponse>> GetFrames(Guid id, int start, int stop);
    }
}
