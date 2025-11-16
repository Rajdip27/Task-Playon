using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlayon.Application.Model.BaseEntities;

namespace TaskPlayon.Core.Model;

public class Invoice: AuditableEntity
{
  
    public long CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<InvoiceItem> InvoiceItem { get; set; } = new List<InvoiceItem>();
    public decimal Total { get; set; }
}
