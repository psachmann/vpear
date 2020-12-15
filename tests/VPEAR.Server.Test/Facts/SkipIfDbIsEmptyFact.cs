using System.Linq;
using VPEAR.Core.Models;
using Xunit;

namespace VPEAR.Server.Test
{
    public class SkipIfDbIsEmptyFact : FactAttribute
    {
        public SkipIfDbIsEmptyFact()
        {
            if (IsEmpty())
            {
                this.Skip = "The db is empty.";
            }
        }

        private bool IsEmpty()
        {
            var context = Mocks.CreateDbContext();

            if (context.Set<Device>().Any())
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
