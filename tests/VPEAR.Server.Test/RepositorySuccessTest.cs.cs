// <copyright file="RepositorySuccessTest.cs.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Server.Data;
using Xunit;

namespace VPEAR.Server.Test
{
    [Collection("DatabaseTest")]
    public class RepositorySuccessTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly IRepository<Device, Guid> devices;

        public RepositorySuccessTest(VPEARDbContextFixture fixture)
        {
            this.devices = new Repository<VPEARDbContext, Device, Guid>(
                fixture.Context,
                Mocks.MockLogger<IRepository<Device, Guid>>().Object);
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            var previousCount = this.devices.Get()
                .ToList()
                .Count;

            var device = new Device();

            var result = await this.devices.CreateAsync(device);

            Assert.NotNull(result);

            var newCount = this.devices.Get()
                .ToList()
                .Count;

            Assert.NotEqual(default, device.Id);
            Assert.Equal(previousCount + 1, newCount);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var deviceToDelete = this.devices.Get()
                .Where(device => device.Status == DeviceStatus.Archived)
                .First();

            var previousCount = this.devices.Get()
                .ToList()
                .Count;

            await this.devices.DeleteAsync(deviceToDelete!);

            var newCount = this.devices.Get()
                .ToList()
                .Count;

            Assert.Equal(previousCount - 1, newCount);
        }

        [Fact]
        public void GetTest()
        {
            var result = this.devices.Get();

            Assert.NotNull(result);
            Assert.InRange(result.Count(), 0, int.MaxValue);
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            var device = this.devices.Get()
                .Where(device => device.Status == DeviceStatus.NotReachable)
                .First();

            var otherDevice = await this.devices.GetAsync(device.Id);

            Assert.NotNull(otherDevice);
            Assert.Equal(device.Id, otherDevice!.Id);
        }

        [Fact]
        public async Task UpdateAsyncTest()
        {
            var device = this.devices.Get()
                .Where(device => device.Status == DeviceStatus.Stopped)
                .First();

            device.Address = "new_address";

            var result = await this.devices.UpdateAsync(device);

            Assert.NotNull(result);

            device = await this.devices.GetAsync(device.Id);

            Assert.NotNull(device);
            Assert.Equal("new_address", device!.Address);
        }
    }
}
