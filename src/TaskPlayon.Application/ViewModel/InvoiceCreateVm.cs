using System.ComponentModel.DataAnnotations;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Application.ViewModel;

public class InvoiceCreateVm
{
    public long Id { get; set; } // Add this for Edit
    // Customer info
    [Required(ErrorMessage = "Customer Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string CustomerName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string CustomerEmail { get; set; } = string.Empty;
    [Required(ErrorMessage = "Phone is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string CustomerPhone { get; set; } = string.Empty;
    [Required(ErrorMessage = "Address is required")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string CustomerAddress { get; set; } = string.Empty;

    // Invoice info
    public DateTimeOffset Date { get; set; } = DateTime.Now;

    public decimal TotalAmount { get; set; }

    // Invoice items
    public List<InvoiceItemVm> Items { get; set; } = new();
}

public class InvoiceItemVm
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}



public class InvoiceVm
{
    public long Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    public decimal Total { get; set; }

    public List<InvoiceItemVmDisplay> Items { get; set; } = new();
}

public class InvoiceItemVmDisplay
{
    public string ProductName { get; set; } = string.Empty;
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}

