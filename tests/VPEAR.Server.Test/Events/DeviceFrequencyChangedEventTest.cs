// <copyright file="DeviceFrequencyChangedEventTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using VPEAR.Core.Entities;
using VPEAR.Core.Events;
using Xunit;

namespace VPEAR.Server.Test.Events
{
    public class DeviceFrequencyChangedEventTest
    {
        [Fact]
        public void ChangedEventTest()
        {
            var device = new Device();

            device.FrequencyChanged(1000);

            Assert.Equal(1, device.Events.Count);
            Assert.IsType<DeviceFrequencyChangedEvent>(device.Events[0]);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void NotChangedEventTest(int? newFrequency)
        {
            var device = new Device();

            device.FrequencyChanged(newFrequency);

            Assert.Equal(0, device.Events.Count);
        }
    }
}
