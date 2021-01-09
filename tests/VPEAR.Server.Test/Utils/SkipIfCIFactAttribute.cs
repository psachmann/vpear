// <copyright file="SkipIfCIFactAttribute.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Xunit;

namespace VPEAR.Server.Test
{
    public class SkipIfCIFactAttribute : FactAttribute
    {
        public SkipIfCIFactAttribute()
        {
#if CI
            this.Skip = "Test doesn't work in CI environment.";
#endif
        }
    }
}
