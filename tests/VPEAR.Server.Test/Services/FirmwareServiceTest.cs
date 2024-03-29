// <copyright file="FirmwareServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class FirmwareServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly IFirmwareService service;

        public FirmwareServiceTest(AutofacFixture fixture)
        {
            this.service = fixture.Container.Resolve<IFirmwareService>();
        }

        [Fact]
        public void GetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.GetAsync(device);

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.NotNull(result.Value);
            });
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var result = await this.service.GetAsync(Mocks.NotExisting.Id);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task GetAsync410GoneTest()
        {
            var result = await this.service.GetAsync(Mocks.Archived.Id);

            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsArchived, result.Error!.Messages);
        }

        [Fact]
        public async Task GetAsync424FailedDependencyTest()
        {
            var result = await this.service.GetAsync(Mocks.NotReachable.Id);

            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, result.Error!.Messages);
        }

        [Theory]
        [InlineData(StatusCodes.Status204NoContent, false, "stable", "next")]
        [InlineData(StatusCodes.Status204NoContent, true, null, null)]
        public void PutAsync204NoContentTest(
            int expectedStatus,
            bool package,
            string? source,
            string? upgrade)
        {
            var devices = new List<Guid>()
            {
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };
            var request = new PutFirmwareRequest()
            {
                Package = package,
                Source = source,
                Upgrade = upgrade,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.PutAsync(device, request);

                Assert.Equal(expectedStatus, result.StatusCode);
                Assert.Null(result.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExisting.Id, new PutFirmwareRequest());

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.service.PutAsync(Mocks.Archived.Id, new PutFirmwareRequest());

            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsArchived, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.service.PutAsync(Mocks.NotReachable.Id, new PutFirmwareRequest());

            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, result.Error!.Messages);
        }
    }
}
