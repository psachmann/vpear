// <copyright file="FilterServiceTest.cs" company="Patrick Sachmann">
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
    public class FilterServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly IFilterService service;

        public FilterServiceTest(AutofacFixture fixture)
        {
            this.service = fixture.Container.Resolve<IFilterService>();
        }

        [Fact]
        public void GetAsync200OKTest()
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
                var result = await this.service.GetAsync(device);

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.NotNull(result.Value);
            });
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var result = await this.service.GetAsync(Mocks.NotExisting.Id);

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
                var result = await this.service.PutAsync(device, new PutFilterRequest());

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Null(result.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExisting.Id, new PutFilterRequest());

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.service.PutAsync(Mocks.Archived.Id, new PutFilterRequest());

            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsArchived, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.service.PutAsync(Mocks.NotReachable.Id, new PutFilterRequest());

            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, result.Error!.Messages);
        }
    }
}
