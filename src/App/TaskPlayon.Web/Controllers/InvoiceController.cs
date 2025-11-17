using Microsoft.AspNetCore.Mvc;
using TaskPlayon.Application.Enums;
using TaskPlayon.Application.Repositories;
using TaskPlayon.Application.Services;
using TaskPlayon.Application.ViewModel;

namespace TaskPlayon.Web.Controllers;

public class InvoiceController(
    IProductRepository productRepo,
    IInvoiceRepository invoiceRepository,
    IPdfService pdfService) : Controller
{
    private readonly IProductRepository _productRepo = productRepo;
    private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
    private readonly IPdfService _pdfService = pdfService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var invoices = await _invoiceRepository.GetAllInvoicesAsync();
        return View(invoices);
    }

    [HttpGet("/invoice/create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Products = await _productRepo.GetAllAsync();
        return View(new InvoiceCreateVm());
    }

    [HttpPost("/invoice/create")]
    public async Task<IActionResult> Create(InvoiceCreateVm vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Products = await _productRepo.GetAllAsync();
            TempData["AlertMessage"] = "Please fix the validation errors.";
            TempData["AlertType"] = "Warning";
            return View(vm);
        }

        try
        {
            bool result = await _invoiceRepository.CreateInvoiceWithCustomerAsync(vm);

            if (result)
            {
                TempData["AlertMessage"] = "Invoice created successfully!";
                TempData["AlertType"] = "Success";
            }
            else
            {
                TempData["AlertMessage"] = "Failed to save invoice. Please try again.";
                TempData["AlertType"] = "Error";
            }
        }
        catch (Exception ex)
        {
            TempData["AlertMessage"] = "An error occurred: " + ex.Message;
            TempData["AlertType"] = "Error";
        }

        return RedirectToAction("Index");
    }

    [HttpGet("/invoice/details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
        var vm = await _invoiceRepository.GetInvoiceByIdAsync(id);
        if (vm == null)
        {
            TempData["AlertMessage"] = $"Invoice with Id {id} not found.";
            TempData["AlertType"] = "Error";
            return RedirectToAction("Index");
        }

        return View(vm);
    }

    [HttpGet("/invoice/edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var vm = await _invoiceRepository.GetInvoiceByIdAsync(id);

        if (vm == null)
        {
            TempData["AlertMessage"] = $"Invoice with Id {id} not found.";
            TempData["AlertType"] = "Error";
            return RedirectToAction("Index");
        }

        ViewBag.Products = await _productRepo.GetAllAsync();
        return View("Create", vm); // reuse Create.cshtml
    }

    [HttpPost("/invoice/edit/{id}")]
    public async Task<IActionResult> Edit(long id, InvoiceCreateVm vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Products = await _productRepo.GetAllAsync();
            TempData["AlertMessage"] = "Please fix the validation errors.";
            TempData["AlertType"] = "Warning";
            return View("Create", vm);
        }

        bool success = await _invoiceRepository.UpdateInvoiceAsync(id, vm);

        if (success)
        {
            TempData["AlertMessage"] = "Invoice updated successfully!";
            TempData["AlertType"] = "Success";
        }
        else
        {
            TempData["AlertMessage"] = "Failed to update invoice.";
            TempData["AlertType"] = "Error";
        }

        return RedirectToAction("Index");
    }

    [HttpGet("/invoice/invoice-pdf/{id}")]
    public async Task<IActionResult> Invoice(long id)
    {
        var vm = await _invoiceRepository.GetInvoiceByIdAsync(id);
        if (vm == null)
        {
            TempData["AlertMessage"] = $"Invoice with Id {id} not found.";
            TempData["AlertType"] = "Error";
            return RedirectToAction("Index");
        }

        byte[] pdf = await _pdfService.GeneratePdfAsync("InvoicePrint", vm, "A4", PaperOrientation.Landscape);
        return File(pdf, "application/pdf", "invoice.pdf");
    }

    [HttpGet("/invoice/get-download-url")]
    public IActionResult GetDownloadUrl(long id)
    {
        string url = Url.Action("Invoice", "Invoice", new { id }, Request.Scheme);
        return Json(new { url });
    }
}
