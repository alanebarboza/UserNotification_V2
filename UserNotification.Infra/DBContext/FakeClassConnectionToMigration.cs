using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserNotification.Infra.DBContext
{
    public class FakeClassConnectionToMigration : IDesignTimeDbContextFactory<SQLDBContext>
    {
        public SQLDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SQLDBContext>();
            optionsBuilder.UseSqlServer("Server=SERVER_HERE;Database=UsersNotifications;User Id=USER_HERE;Password=PASSWORD_HERE;");
            return new SQLDBContext(optionsBuilder.Options);
        }
    }
}
