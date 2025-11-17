using TaskPlayon.Core.Model;

namespace TaskPlayon.Application.ViewModel;

public class InvoiceCreateVm
{
    // Customer info
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;

    // Invoice info
    public DateTime Date { get; set; } = DateTime.Now;

    // Invoice items
    public List<InvoiceItemVm> Items { get; set; } = new();
}

public class InvoiceItemVm
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
