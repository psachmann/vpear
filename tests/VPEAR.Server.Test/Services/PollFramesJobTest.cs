// <copyright file="PollFramesJobTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VPEAR.Core.Entities;
using VPEAR.Server.Services.Jobs;
using Xunit;

namespace VPEAR.Server.Test.Services
{
    public class PollFramesJobTest
    {
        [Fact]
        public async Task PollFramesSucceededTest()
        {
            var context = Mocks.MockJobExecutionContext();
            var devices = Mocks.MockDeviceRepository();
            var frames = Mocks.MockFrameRepository();
            var factory = Mocks.MockDeviceClientFactory(true);
            var logger = Mocks.MockLogger<PollFramesJob>();
            var job = new PollFramesJob(devices.Object, frames.Object, factory.Object, logger.Object);

            await job.Execute(context.Object);
        }

        [Fact]
        public async Task PollFramesFailedTest()
        {
            var context = Mocks.MockJobExecutionContext();
            var devices = Mocks.MockDeviceRepository();
            var frames = Mocks.MockFrameRepository();
            var factory = Mocks.MockDeviceClientFactory(false);
            var logger = Mocks.MockLogger<PollFramesJob>();
            var job = new PollFramesJob(devices.Object, frames.Object, factory.Object, logger.Object);

            await job.Execute(context.Object);

            devices.Verify(mock => mock.UpdateAsync(It.IsAny<Device>()));
        }
    }
}
