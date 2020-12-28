// <copyright file="SensorServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class SensorServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly ISensorService service;

        public SensorServiceTest(AutofacFixture fixture)
        {
            this.service = new SensorService(
                fixture.Container.Resolve<ILogger<SensorController>>(),
                fixture.Container.Resolve<IRepository<Frame, Guid>>(),
                fixture.Container.Resolve<IRepository<Sensor, Guid>>());
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, 0, 0)]
        [InlineData(StatusCodes.Status206PartialContent, 0, int.MaxValue)]
        public void GetFrames2XXTest(int expectedStatus, int start, int stop)
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(device =>
            {
                var result = this.service.GetFrames(device, start, stop);

                Assert.NotNull(result.Value);
                Assert.Equal(expectedStatus, result.StatusCode);
            });
        }

        [Fact]
        public void GetFrames400BadRequestTest()
        {
            var result = this.service.GetFrames(Mocks.Archived.Id, int.MaxValue, int.MinValue);

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.BadRequest, result.Error!.Messages);
        }

        [Fact]
        public void GetFrames404NotFoundTest()
        {
            var result = this.service.GetFrames(Mocks.NotExisting.Id, 0, 0);

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.FramesNotFound, result.Error!.Messages);
        }

        [Fact]
        public void GetSensors200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(device =>
            {
                var result = this.service.GetSensors(device);

                Assert.NotNull(result.Value);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            });
        }

        [Fact]
        public void GetSensors404NotFoundTest()
        {
            var result = this.service.GetSensors(Mocks.NotExisting.Id);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.SensorsNotFound, result.Error!.Messages);
        }
    }
}
