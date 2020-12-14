using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Db;
using Xunit;

namespace VPEAR.Server.Test
{
    [Collection("RepositoryTest")]
    public class RepositoryTest
    {
        private readonly IRepository<Device, Guid> repository;

        public RepositoryTest()
        {
            this.repository = new Repository<VPEARDbContext, Device, Guid>(Mocks.CreateDbContext(),
                Mocks.CreateLogger<IRepository<Device, Guid>>());
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            var previousCount = this.repository.Get()
                .ToList()
                .Count;

            var device = new Device();

            var result = await this.repository.CreateAsync(device);

            var newCount = this.repository.Get()
                .ToList()
                .Count;

            Assert.True(result);
            Assert.NotEqual(default, device.Id);
            Assert.Equal(previousCount + 1, newCount);

        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var previousCount = this.repository.Get()
                .ToList()
                .Count;

            var device = this.repository.Get()
                .Where(d => d.Status == DeviceStatus.Archived)
                .FirstOrDefault();

            Assert.NotNull(device);

            var result = await this.repository.DeleteAsync(device!);

            Assert.True(result);

            var newCount = this.repository.Get()
                .ToList()
                .Count;

            Assert.Equal(previousCount - 1, newCount);
        }

        [Fact]
        public void GetTest()
        {
            var result = this.repository.Get();

            Assert.NotNull(result);
            Assert.InRange(result.Count(), 0, int.MaxValue);
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            var device = await this.repository.Get()
                .LastOrDefaultAsync();

            Assert.NotNull(device);

            var otherDevice = await this.repository.GetAsync(device.Id);

            Assert.NotNull(otherDevice);
            Assert.Equal(device.Id, otherDevice!.Id);
        }

        [Fact]
        public async Task UpdateAsyncTest()
        {
            var device = this.repository.Get()
                .FirstOrDefault();

            Assert.NotNull(device);

            device!.Address = "new_address";

            var result = await this.repository.UpdateAsync(device);

            Assert.True(result);

            device = this.repository.Get()
                .FirstOrDefault();

            Assert.NotNull(device);
            Assert.Equal("new_address", device!.Address);

        }
    }
}
