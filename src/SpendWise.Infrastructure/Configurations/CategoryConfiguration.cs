using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Expenses.Entities;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        // Value Object mappings
        builder.OwnsOne(c => c.CategoryName, cn =>
        {
            cn.Property(x => x.Value)
              .HasColumnName("CategoryName")
              .IsRequired()
              .HasMaxLength(100);
        });

        builder.OwnsOne(c => c.Icon, i =>
        {
            i.Property(x => x.Value)
             .HasColumnName("Icon")
             .HasMaxLength(50);
        });

        // Primitive properties
        builder.Property(c => c.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.CreatedByUserId)
               .IsRequired();

        // Relationships
        builder.HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Expenses)
               .WithOne(e => e.Category)
               .HasForeignKey(e => e.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        // Use backing field for Expenses collection
        builder.Navigation(c => c.Expenses)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
