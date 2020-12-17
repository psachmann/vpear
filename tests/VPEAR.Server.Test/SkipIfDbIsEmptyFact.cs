using System.Linq;
using VPEAR.Core.Models;
using VPEAR.Server.Db;
using Xunit;

namespace VPEAR.Server.Test
{
    public class SkipIfDbIsEmptyFact : FactAttribute, IClassFixture<VPEARDbContextFixture>
    {
        private readonly VPEARDbContext context;

        public SkipIfDbIsEmptyFact(VPEARDbContextFixture fixture)
        {
            this.context = fixture.Context;

            if (IsEmpty())
            {
                this.Skip = "The db is empty.";
            }
        }

        private bool IsEmpty()
        {
            if (this.context.Set<Device>().Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
