using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.ValueObjects;

namespace SpendWise.Infrastructure.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply migrations
        await context.Database.MigrateAsync();

        var adminEmail = "janmors13@gmail.com";
        var adminPassword = "Admin123";

        var existingAdmin = await context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == adminEmail);

        if (existingAdmin != null)
        {
            Console.WriteLine("Admin user already exists. Skipping seeding.");
            return;
        }

        var adminUserResult = User.Create(
            Guid.NewGuid(),
            "System",
            "Admin",
            25,
            adminEmail,
            null,
            PasswordHash.FromPlainText(adminPassword).Value
        );

        if (adminUserResult.IsFailure)
            throw new Exception($"Admin creation failed: {adminUserResult.Error}");

        var adminUser = adminUserResult.Value;

        var promoteResult = adminUser.PromoteToAdmin();
        if (promoteResult.IsFailure)
            throw new Exception($"Failed to promote user to admin: {promoteResult.Error}");

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        Console.WriteLine("Admin user created successfully.");
    }
}
