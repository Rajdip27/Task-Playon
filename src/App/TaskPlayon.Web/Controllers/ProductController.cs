using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskPlayon.Application.Repositories;
using TaskPlayon.Application.ViewModel;
using TaskPlayon.Core.Model;
using TaskPlayon.Application.Logging;

namespace TaskPlayon.Web.Controllers;
public class ProductController(
    IProductRepository productRepository,
    IMapper mapper,
    IAppLogger<ProductController> logger) : Controller
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<ProductController> _logger = logger;

    [HttpGet("/product")]
    public async Task<IActionResult> Index()
    {
        try
        {
            #if DEBUG
            _logger.LogInfo("Start Watch");
            var stopwatch = Stopwatch.StartNew();
            #endif
            _logger.LogInfo($"Fetching products.");
            var pagination = await _productRepository.GetAllAsync();
            #if DEBUG
            _logger.LogInfo($"GetProducts took {stopwatch.ElapsedMilliseconds}ms");
           #endif
            _logger.LogInfo($"Fetched {pagination.Count()} products");

            return View(pagination);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching products", ex);
            return StatusCode(500, "An error occurred while fetching products.");
        }
    }
    [HttpGet("/product/create-or-edit/{id?}")]
    public async Task<IActionResult> CreateOrEdit(long id = 0)
    {
        try
        {
            if (id > 0)
            {
                _logger.LogInfo($"Editing Product Id={id}");

                var entity = await _productRepository.FirstOrDefaultAsync(id);

                if (entity == null)
                {
                    TempData["AlertMessage"] = $"Product with Id {id} not found.";
                    TempData["AlertType"] = "Error";
                    return RedirectToAction(nameof(Index));
                }

                return View(_mapper.Map<ProductVm>(entity));
            }

            return View(new ProductVm());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in CreateOrEdit for Id={id}", ex);
            return StatusCode(500, "An error occurred while opening the form.");
        }
    }
    [HttpPost("/product/create-or-edit/{id?}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(ProductVm productVm)
    {
        if (!ModelState.IsValid)
        {
            TempData["AlertMessage"] = "Please fix validation errors.";
            TempData["AlertType"] = "Warning";
            return View(productVm);
        }

        try
        {
            var entity = _mapper.Map<Product>(productVm);

            if (productVm.Id == 0)
            {
                await _productRepository.InsertAsync(entity);
                TempData["AlertMessage"] = "Product created successfully!";
            }
            else
            {
                var updated = await _productRepository.UpdateAsync(productVm.Id, entity);

                if (updated == null)
                {
                    TempData["AlertMessage"] = $"Product with Id {productVm.Id} not found.";
                    TempData["AlertType"] = "Error";
                    return NotFound();
                }

                TempData["AlertMessage"] = "Product updated successfully!";
            }

            TempData["AlertType"] = "Success";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while creating/updating product", ex);
            TempData["AlertMessage"] = "An error occurred while saving the product.";
            TempData["AlertType"] = "Error";
            return StatusCode(500);
        }
    }
    [HttpPost("/product/delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var deleted = await _productRepository.DeleteAsync(id);

            if (deleted is null)
            {
                TempData["AlertMessage"] = $"Product with Id {id} not found.";
                TempData["AlertType"] = "Error";
                return NotFound();
            }

            TempData["AlertMessage"] = "Product deleted successfully!";
            TempData["AlertType"] = "Success";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting product Id={id}", ex);
            TempData["AlertMessage"] = "An error occurred while deleting the product.";
            TempData["AlertType"] = "Error";
            return StatusCode(500);
        }
    }
}
