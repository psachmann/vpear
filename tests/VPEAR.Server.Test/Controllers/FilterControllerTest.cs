using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
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
        private readonly Guid notExistingDevice;
        private readonly FiltersController controller;

        public FilterControllerTest(VPEARDbContextFixture fixture)
        {
            var logger = Mocks.CreateLogger<FiltersController>();
            this.controller = new FiltersController(logger, new FilterService(logger,
                Mocks.CreateRepository<Device, Guid>(fixture.Context),
                Mocks.CreateRepository<Filter, Guid>(fixture.Context)));
        }

        [Fact]
        public async Task OnGetAsync2XXTest()
        {
            var response = await this.controller.OnGetAsync(this.recordingDevice);
            var result = Assert.IsType<JsonResult>(response);

            Assert.IsAssignableFrom<GetFiltersResponse>(result);
        }

        public async Task OnPutAsync()
        {

        }
    }
}
