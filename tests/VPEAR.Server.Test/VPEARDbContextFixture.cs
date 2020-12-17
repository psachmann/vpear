using Microsoft.EntityFrameworkCore;
using System;
using VPEAR.Server.Db;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class VPEARDbContextFixture : IDisposable
    {
        public VPEARDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<VPEARDbContext>()
                .UseInMemoryDatabase(Schemas.DbSchema)
                .Options;

            this.Context = new VPEARDbContext(options);
            this.Context.Database.EnsureCreated();
        }

        public VPEARDbContext Context { get; private set; }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
