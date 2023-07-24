using Microsoft.EntityFrameworkCore;

namespace DDD.Core.Infrastructure.Data
{
    public class FakeDbContext : DbBoundedContext
    {
        #region Constructors

        public FakeDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructors
    }
}
