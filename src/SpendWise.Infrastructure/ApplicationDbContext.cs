using Microsoft.EntityFrameworkCore;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}
