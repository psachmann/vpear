// <copyright file="DeviceStatusChangedEventTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using VPEAR.Core;
using VPEAR.Core.Entities;
using VPEAR.Core.Events;
using Xunit;

namespace VPEAR.Server.Test.Events
{
    public class DeviceStatusChangedEventTest
    {
        [Fact]
        public void ChangedEventTest()
        {
            var device = new Device();

            device.StatusChanged(DeviceStatus.Archived);

            Assert.Equal(1, device.Events.Count);
            Assert.IsType<DeviceStatusChangedEvent>(device.Events[0]);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(DeviceStatus.Stopped)]
        public void NotChangedEventTest(DeviceStatus? newStatus)
        {
            var device = new Device();

            device.StatusChanged(newStatus);

            Assert.Equal(0, device.Events.Count);
        }
    }
}
