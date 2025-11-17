using Microsoft.AspNetCore.Mvc;
using TaskPlayon.Application.Repositories;
using TaskPlayon.Application.ViewModel;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Web.Controllers;

public class InvoiceController(IProductRepository productRepo,IInvoiceRepository invoiceRepository) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var invoices = await invoiceRepository.GetAllInvoicesAsync();
        return View(invoices);
    }

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
        if (!ModelState.IsValid)
        {
            ViewBag.Products = await productRepo.GetAllAsync(); // reload products
            return View(vm);
        }

        try
        {
            bool result = await invoiceRepository.CreateInvoiceWithCustomerAsync(vm);

            if (result)
            {
                TempData["Success"] = "Invoice created successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to save invoice. Please try again.";
            }
        }
        catch (Exception ex)
        {
            // Optionally log the error: ex.Message
            TempData["Error"] = "An error occurred: " + ex.Message;
        }

        return RedirectToAction("Index");
    }


    [HttpGet("/invoice/details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
        var vm = await invoiceRepository.GetInvoiceByIdAsync(id);
        if (vm == null) return NotFound();

        return View(vm);
    }


    [HttpGet("/invoice/edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var invoice = await invoiceRepository.GetInvoiceByIdAsync(id); // returns InvoiceCreateVm

        if (invoice == null) return NotFound();

        // Map InvoiceDetailsVm to InvoiceCreateVm if needed
        var vm = new InvoiceCreateVm
        {
            CustomerName = invoice.CustomerName,
            CustomerEmail = invoice.CustomerEmail,
            CustomerPhone = invoice.CustomerPhone,
            CustomerAddress = invoice.CustomerAddress,
            Id = invoice.Id,
            Date = invoice.Date,
            TotalAmount = invoice.Total,
            Items = invoice.Items.Select(i => new InvoiceItemVm
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        ViewBag.Products = await productRepo.GetAllAsync();

        return View("Create", vm); // reuse the Create.cshtml view
    }

    [HttpPost("/invoice/edit/{id}")]
    public async Task<IActionResult> Edit(long id, InvoiceCreateVm vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Products = await productRepo.GetAllAsync();
            return View("InvoiceForm", vm);
        }

        bool success = await invoiceRepository.UpdateInvoiceAsync(id, vm);

        if (success)
            TempData["Success"] = "Invoice updated successfully!";
        else
            TempData["Error"] = "Failed to update invoice.";

        return RedirectToAction("Index");
    }


}



