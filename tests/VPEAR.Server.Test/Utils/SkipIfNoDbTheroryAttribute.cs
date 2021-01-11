// <copyright file="SkipIfNoDbTheroryAttribute.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using VPEAR.Server.Data;
using Xunit;

namespace VPEAR.Server.Test
{
    public class SkipIfNoDbTheroryAttribute : TheoryAttribute
    {
        private static readonly DesignTimeVPEARDbContextFactory ContextFactory = new DesignTimeVPEARDbContextFactory();

        public SkipIfNoDbTheroryAttribute()
        {
            using var context = ContextFactory.CreateDbContext(Environment.GetCommandLineArgs());

            if (!context.Database.CanConnect())
            {
                this.Skip = "Test doesn't work in CI environment.";
            }
        }
    }
}
