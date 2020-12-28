// <copyright file="WifiServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
    public class WifiServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly IWifiService service;

        public WifiServiceTest(AutofacFixture fixture)
        {
            this.service = new WifiService(
                fixture.Container.Resolve<ILogger<WifiController>>(),
                fixture.Container.Resolve<IRepository<Device, Guid>>(),
                fixture.Container.Resolve<IRepository<Wifi, Guid>>(),
                fixture.Container.Resolve<IDeviceClient.Factory>());
        }

        [Fact]
        public void Get200OKTest()
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
                var result = this.service.Get(device);

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.NotNull(result.Value);
            });
        }

        [Fact]
        public void Get404NotFoundTest()
        {
            var result = this.service.Get(Mocks.NotExisting.Id);

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public void PutAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.PutAsync(device, new PutWifiRequest());

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Null(result.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NoFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExisting.Id, new PutWifiRequest());;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.service.PutAsync(Mocks.Archived.Id, new PutWifiRequest());

            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsArchived, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.service.PutAsync(Mocks.NotReachable.Id, new PutWifiRequest());

            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, result.Error!.Messages);
        }
    }
}
