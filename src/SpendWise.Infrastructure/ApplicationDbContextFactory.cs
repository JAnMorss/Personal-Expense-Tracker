using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SpendWise.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SpendWiseDb;User Id=sa;Password=root;Trusted_Connection=False;TrustServerCertificate=true"
        );

        return new ApplicationDbContext(optionsBuilder.Options, publisher: null!);
    }
}
