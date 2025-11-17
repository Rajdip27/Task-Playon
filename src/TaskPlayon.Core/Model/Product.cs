using TaskPlayon.Application.Model.BaseEntities;

namespace TaskPlayon.Core.Model;

public class Product: AuditableEntity
{

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public ICollection<InvoiceItem> InvoiceItem { get; set; } = new List<InvoiceItem>();
}
