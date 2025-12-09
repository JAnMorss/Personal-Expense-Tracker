using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        // Value Object mappings
        builder.OwnsOne(u => u.FirstName, fn =>
        {
            fn.Property(f => f.Value)
              .HasColumnName("FirstName")
              .IsRequired()
              .HasMaxLength(20);
        });

        builder.OwnsOne(u => u.LastName, ln =>
        {
            ln.Property(l => l.Value)
              .HasColumnName("LastName")
              .IsRequired()
              .HasMaxLength(15);
        });

        builder.OwnsOne(u => u.Age, a =>
        {
            a.Property(a => a.Value)
             .HasColumnName("Age")
             .IsRequired();
        });

        builder.OwnsOne(u => u.Email, e =>
        {
            e.Property(em => em.Value)
             .HasColumnName("Email")
             .IsRequired()
             .HasMaxLength(255);

            e.HasIndex(em => em.Value).IsUnique();
        });

        builder.OwnsOne(u => u.Avatar, a =>
        {
            a.Property(av => av.Value)
             .HasColumnName("Avatar")
             .HasMaxLength(500);
        });

        builder.OwnsOne(u => u.PasswordHash, ph =>
        {
            ph.Property(p => p.Value)
              .HasColumnName("PasswordHash")
              .IsRequired()
              .HasMaxLength(500);
        });

        // Primitive properties
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

    }
}
