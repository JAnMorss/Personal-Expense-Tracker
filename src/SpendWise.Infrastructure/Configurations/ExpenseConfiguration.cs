using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendWise.Domain.Expenses.Entities;

namespace SpendWise.Infrastructure.Configurations;

internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");

        builder.HasKey(e => e.Id);

        // Value Object mappings
        builder.OwnsOne(e => e.Amount, a =>
        {
            a.Property(x => x.Value)
             .HasColumnName("Amount")
             .IsRequired()
             .HasColumnType("decimal(18,2)");
        });

        builder.OwnsOne(e => e.Description, d =>
        {
            d.Property(x => x.Value)
             .HasColumnName("Description")
             .HasMaxLength(500);
        });

        // Primitive properties
        builder.Property(e => e.Date)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.CreatedByUserId)
               .IsRequired();

        builder.Property(e => e.CategoryId)
               .IsRequired();

        // Relationships
        builder.HasOne(e => e.Category)
               .WithMany(c => c.Expenses)
               .HasForeignKey(e => e.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
               .WithMany()
               .HasForeignKey(e => e.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
