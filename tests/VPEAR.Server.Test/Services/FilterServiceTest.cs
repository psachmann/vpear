// <copyright file="FilterServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

/*
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;

namespace VPEAR.Server.Test.Services
{
    public class FilterServiceTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly Guid notExistingDevice = new Guid();
        private readonly IFilterService service;

        public FilterServiceTest(VPEARDbContextFixture fixture)
        {
            this.service = new FilterService(
                Mocks.CreateLogger<FiltersController>(),
                Mocks.CreateRepository<Device, Guid>(fixture.Context),
                Mocks.CreateRepository<Filter, Guid>(fixture.Context));
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                this.archivedDevice,
                this.notReachableDevice,
                this.stoppedDevice,
                this.recordingDevice,
            };

            foreach (var device in devices)
            {
                var response = await this.service.GetAsync(device);

                Assert.NotNull(response.Payload);
                Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var response = await this.service.GetAsync(this.notExistingDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync200OKTest()
        {
            var response = await this.service.PutAsync(this.recordingDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var response = await this.service.PutAsync(this.notExistingDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync410FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.archivedDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.notReachableDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
        }
    }
}
*/
