using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TaskPlayon.Application.Model.BaseEntities;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Application.ViewModel;

[AutoMap(typeof(Product), ReverseMap = true)]
public class ProductVm:BaseEntity
{

    [Required(ErrorMessage = "Product Name is required")]
    [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    public int Stock { get; set; }
}
