// <copyright file="FilterServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class FilterServiceTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly IFilterService service;

        public FilterServiceTest(VPEARDbContextFixture fixture)
        {
            this.service = new FilterService(
                Mocks.CreateLogger<FiltersController>(),
                Mocks.CreateRepository<Device, Guid>(fixture.Context),
                Mocks.CreateRepository<Filter, Guid>(fixture.Context));
        }

        [Fact]
        public void GetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.ArchivedDeviceId,
                Mocks.NotReachableDeviceId,
                Mocks.StoppedDeviceId,
                Mocks.RecordingDeviceId,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.GetAsync(device);

                Assert.True(result.IsSuccess);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.NotNull(result.Value);
            });
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var result = await this.service.GetAsync(Mocks.NotExistingDeviceId);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.Error!.StatusCode);
            Assert.Equal(ErrorMessages.DeviceNotFound, result.Error!.Message);
        }

        [Fact]
        public void PutAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.StoppedDeviceId,
                Mocks.RecordingDeviceId,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.PutAsync(device, new PutFiltersRequest());

                Assert.True(result.IsSuccess);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.NotNull(result.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExistingDeviceId, new PutFiltersRequest());

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.Error!.StatusCode);
            Assert.Equal(ErrorMessages.DeviceNotFound, result.Error!.Message);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.service.PutAsync(Mocks.ArchivedDeviceId, new PutFiltersRequest());

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status410Gone, result.Error!.StatusCode);
            Assert.Equal(ErrorMessages.DeviceIsArchived, result.Error!.Message);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.service.PutAsync(Mocks.NotReachableDeviceId, new PutFiltersRequest());

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status424FailedDependency, result.Error!.StatusCode);
            Assert.Equal(ErrorMessages.DeviceIsNotReachable, result.Error!.Message);
        }
    }
}
