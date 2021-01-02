// <copyright file="RepositoryFailureTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Db;
using Xunit;

namespace VPEAR.Server.Test
{
    [Collection("RepositoryTest")]
    public class RepositoryFailureTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly IRepository<Device, Guid> devices;

        public RepositoryFailureTest(VPEARDbContextFixture fixture)
        {
            this.devices = new Repository<VPEARDbContext, Device, Guid>(
                fixture.Context,
                Mocks.CreateLogger<IRepository<Device, Guid>>());

            // provokes the failure of every db operation to trigger error handling
            fixture.Context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            var device = new Device();

            Assert.False(await this.devices.CreateAsync(device));
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var device = new Device();

            Assert.False(await this.devices.DeleteAsync(device));
        }

        [Fact]
        public void GetTest()
        {
            var result = this.devices.Get();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            var device = await this.devices.GetAsync(new Guid("00000000000000000000000000000000"));

            Assert.Null(device);
        }

        [Fact]
        public async Task UpdateAsyncFailureTest()
        {
            var device = new Device();

            Assert.False(await this.devices.UpdateAsync(device));
        }
    }
}