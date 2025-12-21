using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Categories.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.CategoryName, cn =>
        {
            cn.Property(p => p.Value)
                .HasColumnName("CategoryName")
                .IsRequired()
                .HasMaxLength(40);
        });

        builder.OwnsOne(c => c.Icon, icon =>
        {
            icon.Property(p => p.Value)
                .HasColumnName("Icon")
                .HasMaxLength(20)
                .HasDefaultValue(""); 
        });


        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        builder.Property(c => c.CreatedByUserId)
            .IsRequired();

        builder.HasIndex(c => c.CreatedByUserId);

        builder.Metadata
            .FindNavigation(nameof(Category.Expenses))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
