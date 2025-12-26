using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(rolePermission => new { rolePermission.RoleId, rolePermission.PermissionId });

        builder.HasData(
            new RolePermission
            {
                RoleId = Role.Registered.Id,
                PermissionId = Permission.UserRead.Id,
            });
    }
}
