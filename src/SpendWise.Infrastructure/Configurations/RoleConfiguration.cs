using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(role => role.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.HasMany(role => role.Users)
            .WithMany(role => role.Roles)
            .UsingEntity(r => r.ToTable("UserRoles"));

       builder.HasData(
            new Role(Role.Registered.Id, Role.Registered.Name),
            new Role(Role.Admin.Id, Role.Admin.Name)
        );
    }
}
