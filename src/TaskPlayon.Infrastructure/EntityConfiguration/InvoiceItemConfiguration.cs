using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Infrastructure.EntityConfiguration;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable(nameof(InvoiceItem));
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Product)
       .WithMany(x => x.InvoiceItem)
       .HasForeignKey(x => x.ProductId);

        builder.HasOne(x => x.Invoice)
       .WithMany(x => x.InvoiceItem)
       .HasForeignKey(x => x.InvoiceId);
    }
}
