// <copyright file="DeviceFrequencyChangedHandlerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Moq;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core.Entities;
using VPEAR.Core.Events;
using VPEAR.Server.Handlers;
using Xunit;

namespace VPEAR.Server.Test.Events
{
    public class DeviceFrequencyChangedHandlerTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public async Task UpdatePollFramesJobTest(int frequency)
        {
            var scheduler = Mocks.MockScheduler();
            var factory = Mocks.CreateSchedulerFactory(scheduler);
            var logger = Mocks.MockLogger<DeviceFrequencyChangedHandler>();
            var handler = new DeviceFrequencyChangedHandler(factory, logger.Object);
            var device = new Device()
            {
                Frequency = frequency,
                Status = Core.DeviceStatus.Recording,
            };

            await handler.Handle(new DeviceFrequencyChangedEvent(device), default);

            scheduler.Verify(mock => mock.DeleteJob(It.IsAny<JobKey>(), It.IsAny<CancellationToken>()));
            scheduler.Verify(mock => mock.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task NotUpdatePollFramesJobTest()
        {
            var scheduler = Mocks.MockScheduler();
            var factory = Mocks.CreateSchedulerFactory(scheduler);
            var logger = Mocks.MockLogger<DeviceFrequencyChangedHandler>();
            var handler = new DeviceFrequencyChangedHandler(factory, logger.Object);
            var device = new Device();

            await handler.Handle(new DeviceFrequencyChangedEvent(device), default);

            scheduler.Verify(mock => mock.DeleteJob(It.IsAny<JobKey>(), It.IsAny<CancellationToken>()));
            scheduler.VerifyNoOtherCalls();
        }
    }
}
