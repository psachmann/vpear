// <copyright file="SensorServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class SensorServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly ISensorService service;

        public SensorServiceTest(AutofacFixture fixture)
        {
            this.service = fixture.Container.Resolve<ISensorService>();
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, 0, 0)]
        [InlineData(StatusCodes.Status206PartialContent, 0, int.MaxValue)]
        public void GetFramesAsync2XXTest(int expectedStatus, int start, int stop)
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.GetFramesAsync(device, start, stop);

                Assert.NotNull(result.Value);
                Assert.Equal(expectedStatus, result.StatusCode);
            });
        }

        [Fact]
        public async Task GetFramesAsync400BadRequestTest()
        {
            var result = await this.service.GetFramesAsync(Mocks.Archived.Id, int.MaxValue, int.MinValue);

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.BadRequest, result.Error!.Messages);
        }

        [Fact]
        public async Task GetFramesAsnyc404NotFoundTest()
        {
            var result = await this.service.GetFramesAsync(Mocks.NotExisting.Id, 0, 0);

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public void GetSensorsAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.GetSensorsAsync(device);

                Assert.NotNull(result.Value);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            });
        }

        [Fact]
        public async Task GetSensorsAsync404NotFoundTest()
        {
            var result = await this.service.GetSensorsAsync(Mocks.NotExisting.Id);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }
    }
}
