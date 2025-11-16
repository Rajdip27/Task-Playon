using System.ComponentModel.DataAnnotations;
using TaskPlayon.Application.Model.BaseEntities;

namespace TaskPlayon.Core.Model;

public class InvoiceItem:AuditableEntity
{
    public long InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
