using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Categories.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories"); 

        builder.HasKey(x => x.Id);

        builder.OwnsOne(c => c.CategoryName, cn =>
        {
            cn.Property(c => c.Value)
              .HasColumnName("CategoryName")  
              .IsRequired()
              .HasMaxLength(40);  
        });

        builder.OwnsOne(c => c.Icon, icon =>
        {
            icon.Property(i => i.Value)
                .HasColumnName("Icon")
                .HasMaxLength(20) 
                .IsRequired(false);  
        });

        // Primitive properties
        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()"); 

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);  

        builder.Property(c => c.CreatedByUserId)
            .IsRequired();  

        builder.HasIndex(c => c.CreatedByUserId);
    }
}

