using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;

namespace VPEAR.Server.Test
{
    [Collection("SensorServiceTest")]
    public class SensorServiceTest
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly Guid notExistingDevice = new Guid();
        private readonly VPEARDbContext context;
        private readonly ISensorService service;

        public SensorServiceTest()
        {
            this.context = Mocks.CreateDbContext();
            this.service = new SensorService(
                Mocks.CreateLogger<SensorController>(),
                Mocks.CreateRepository<Frame, Guid>(this.context),
                Mocks.CreateRepository<Sensor, Guid>(this.context));
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null, null)]
        [InlineData(StatusCodes.Status200OK, uint.MinValue, null)]
        [InlineData(StatusCodes.Status204NoContent, uint.MaxValue, uint.MinValue)]
        [InlineData(StatusCodes.Status206PartialContent, uint.MinValue, uint.MaxValue)]
        public async Task GetFramesAsync2XXTest(int expectedStatus, uint? start, uint? stop)
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
                var response = await this.service.GetFramesAsync(device, start, stop);

                Assert.NotNull(response.Payload);
                Assert.Equal(expectedStatus, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetFramesAsync404NotFoundTest()
        {
            var response = await this.service.GetFramesAsync(this.notExistingDevice, null, null);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetSensorsAsync200OKTest()
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
                var response = await this.service.GetSensorsAsync(device);

                Assert.NotNull(response.Payload);
                Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetSensorsAsync404NotFoundTest()
        {
            var response = await this.service.GetSensorsAsync(this.notExistingDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }
    }
}
