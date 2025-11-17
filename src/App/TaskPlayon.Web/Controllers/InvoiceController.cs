using Microsoft.AspNetCore.Mvc;
using TaskPlayon.Application.Repositories;
using TaskPlayon.Application.ViewModel;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Web.Controllers;

public class InvoiceController(IProductRepository productRepo) : Controller
{
    [HttpGet("/invoice/create")]
    public async Task<IActionResult> Create()
    {
        var data= await productRepo.GetAllAsync();
        ViewBag.Products = data;  // for dropdown
        return View(new InvoiceCreateVm());
    }

    [HttpPost("/invoice/create")]
    public async Task<IActionResult> Create(InvoiceCreateVm vm)
    {
        return RedirectToAction("Index");
    }
}



//if (!ModelState.IsValid)
//{
//    ViewBag.Products = productRepo.GetAll();
//    return View(vm);
//}

//// save customer
//var customer = new Customer
//{
//    Name = vm.CustomerName,
//    Email = vm.CustomerEmail,
//    Phone = vm.CustomerPhone,
//    Address = vm.CustomerAddress
//};

//await customerRepo.InsertAsync(customer);

//// prepare invoice
//var invoice = new Invoice
//{
//    CustomerId = customer.Id,
//    Date = vm.Date,
//    Total = vm.Items.Sum(x => x.UnitPrice * x.Quantity)
//};

//await invoiceRepo.InsertAsync(invoice);

//// save invoice items
//foreach (var item in vm.Items)
//{
//    var invItem = new InvoiceItem
//    {
//        InvoiceId = invoice.Id,
//        ProductId = item.ProductId,
//        Quantity = item.Quantity,
//        UnitPrice = item.UnitPrice
//    };

//    await invoiceRepo.AddItem(invItem);
//}