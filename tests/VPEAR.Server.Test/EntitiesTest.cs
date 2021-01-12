// <copyright file="EntitiesTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using VPEAR.Core.Extensions;
using VPEAR.Core.Entities;
using Xunit;

namespace VPEAR.Server.Test
{
    public class EntitiesTest
    {
        [Fact]
        public void GetterSetterTest()
        {
            var device = new Device();
            var filter = new Filter();
            var frame = new Frame();

            Assert.Equal(device.ToJsonString(), device.Clone().ToJsonString());
            Assert.Equal(filter.ToJsonString(), filter.Clone().ToJsonString());
            Assert.Equal(frame.ToJsonString(), frame.Clone().ToJsonString());
        }
    }
}
