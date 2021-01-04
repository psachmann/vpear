// <copyright file="IEventDetector.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    public interface IEventDetector<TDbContext>
    {
        void Detect(TDbContext context);

        Task DetectAsync(TDbContext context);
    }
}
