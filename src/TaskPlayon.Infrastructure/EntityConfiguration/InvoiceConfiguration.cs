using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Infrastructure.EntityConfiguration;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable(nameof(Invoice));
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Customer)
          .WithMany(x =>x.Invoices)
          .HasForeignKey(x => x.CustomerId);

    }
}
