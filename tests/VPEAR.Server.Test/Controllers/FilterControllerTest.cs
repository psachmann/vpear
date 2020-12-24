// <copyright file="FilterControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

/*
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class FilterControllerTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly FiltersController controller;

        public FilterControllerTest(VPEARDbContextFixture fixture)
        {
            var logger = Mocks.CreateLogger<FiltersController>();
            this.controller = new FiltersController(logger, new FilterService(logger,
                Mocks.CreateRepository<Device, Guid>(fixture.Context),
                Mocks.CreateRepository<Filter, Guid>(fixture.Context)));
        }

        [Fact]
        public void OnGetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                this.archivedDevice,
                this.notReachableDevice,
                this.recordingDevice,
                this.stoppedDevice,
            };

            devices.ForEach(async d =>
            {
                var response = await this.controller.OnGetAsync(d);
                var result = Assert.IsType<JsonResult>(response);

                Assert.IsAssignableFrom<GetFiltersResponse>(result.Value);
            });
        }

        [Fact]
        public async Task OnGetAsync404NotFound()
        {
            var response = await this.controller.OnGetAsync(new Guid());
            var result = Assert.IsType<JsonResult>(response);

            Assert.Null(result.Value);
        }
    }
}
*/
