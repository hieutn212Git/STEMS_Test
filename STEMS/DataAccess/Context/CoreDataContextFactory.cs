using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Context
{
    public class CoreDataContextFactory : IDesignTimeDbContextFactory<CoreDataContext>
    {
        public CoreDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDataContext>();

            return new CoreDataContext(optionsBuilder.Options);
        }
    }
}
