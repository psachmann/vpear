// <copyright file="FilterServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class FilterServiceTest
    {
        private readonly IFilterService service;

        public FilterServiceTest()
        {
            this.service = new FilterService(
                Mocks.CreateLogger<FiltersController>(),
                Mocks.CreateRepository<Device>(),
                Mocks.CreateRepository<Filter>());
        }

        [Fact]
        public void GetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.ArchivedDeviceId.Id,
                Mocks.NotReachableDeviceId.Id,
                Mocks.StoppedDeviceId.Id,
                Mocks.RecordingDeviceId.Id,
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
            var result = await this.service.GetAsync(Mocks.NotExistingDeviceId);

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorMessages.DeviceNotFound, result.Error!.Message);
        }

        [Fact]
        public void PutAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.StoppedDeviceId.Id,
                Mocks.RecordingDeviceId.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.PutAsync(device, new PutFiltersRequest());

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Null(result.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExistingDeviceId, new PutFiltersRequest());

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorMessages.DeviceNotFound, result.Error!.Message);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.service.PutAsync(Mocks.ArchivedDeviceId.Id, new PutFiltersRequest());

            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorMessages.DeviceIsArchived, result.Error!.Message);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.service.PutAsync(Mocks.NotReachableDeviceId.Id, new PutFiltersRequest());

            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorMessages.DeviceIsNotReachable, result.Error!.Message);
        }
    }
}
