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

        builder.OwnsOne(e => e.Amount, a =>
        {
            a.Property(p => p.Value)
             .HasColumnName("Amount")
             .IsRequired()
             .HasPrecision(18, 2);
        });

        builder.OwnsOne(e => e.Description, d =>
        {
            d.Property(p => p.Value)
             .HasColumnName("Description")
             .HasMaxLength(500)
             .HasDefaultValue(""); 
        });


        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedByUserId)
            .IsRequired();

        builder.Property(e => e.CategoryId)
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CreatedByUserId);
        builder.HasIndex(e => e.CategoryId);
        builder.HasIndex(e => e.Date);
    }
}
