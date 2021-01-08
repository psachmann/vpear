// <copyright file="FilterControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class FilterControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly FilterController controller;

        public FilterControllerTest(AutofacFixture fixture)
        {
            this.controller = fixture.Container.Resolve<FilterController>();
        }

        [Fact]
        public void OnGetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnGetAsync(device);
                var objectResult = Assert.IsType<ObjectResult>(result);
                var response = Assert.IsAssignableFrom<GetFiltersResponse>(objectResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public async Task OnGetAsync404NotFound()
        {
            var result = await this.controller.OnGetAsync(Mocks.NotExisting.Id);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public void OnPutAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnPutAsync(device, new PutFilterRequest());
                var objectResult = Assert.IsType<ObjectResult>(result);

                Assert.Null(objectResult.Value);
            });
        }

        [Fact]
        public async Task OnPutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotExisting.Id, new PutFilterRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task OnPutAsync410GoneTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.Archived.Id, new PutFilterRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task OnPutAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotReachable.Id, new PutFilterRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }
    }
}
