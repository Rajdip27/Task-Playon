using Microsoft.AspNetCore.Mvc;
using TaskPlayon.Application.Enums;
using TaskPlayon.Application.Repositories;
using TaskPlayon.Application.Services;
using TaskPlayon.Application.ViewModel;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Web.Controllers;

public class InvoiceController(IProductRepository productRepo,IInvoiceRepository invoiceRepository, IPdfService pdfService) : Controller
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
        var vm = await invoiceRepository.GetInvoiceByIdAsync(id);

        if (vm == null) return NotFound();

        ViewBag.Products = await productRepo.GetAllAsync();

        return View("Create", vm); // reuse Create.cshtml
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


    [HttpGet("/invoice/invoice-pdf/{id}")]
    public async Task<IActionResult> Invoice(long id)
    {
        var vm = await invoiceRepository.GetInvoiceByIdAsync(id);
        byte[] pdf = await pdfService.GeneratePdfAsync("InvoicePrint", vm, "A4", PaperOrientation.Landscape);
        return File(pdf, "application/pdf", "invoice.pdf");
    }

    // New AJAX endpoint - returns download URL
    [HttpGet("/invoice/get-download-url")]
    public IActionResult GetDownloadUrl(long id)
    {
        string url = Url.Action("Invoice", "Invoice", new { id }, Request.Scheme);
        return Json(new { url });
    }


}



