using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Areas.Masters.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService, ILookupService lookupService)
        {
            _logger = logger;
            _productService = productService;
            _lookupService = lookupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProductsData([FromQuery] ProductFilter filter)
        {
            var query = _productService.GetAll("Category,Unit").AsQueryable().Where(x => x.Type == (int)ProductTypes.Product);

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name != null && x.Name.ToLower().Contains(filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Make))
                query = query.Where(x => x.Make != null && x.Make.ToLower().Contains(filter.Make.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Pack))
                query = query.Where(x => x.Pack != null && x.Pack.ToLower().Contains(filter.Pack.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Hsn))
                query = query.Where(x => x.Hsn != null && x.Hsn.ToLower().Contains(filter.Hsn.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
                query = query.Where(x => x.Category.Name != null && x.Category.Name.ToLower().Contains(filter.CategoryName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.UnitName))
                query = query.Where(x => x.Unit.Name != null && x.Unit.Name.ToLower().Contains(filter.UnitName.ToLower()));

            if (filter.Active > -1)
                query = query.Where(x => x.Active == filter.Active);

            int totalProducts = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Type = ProductTypes.Product,
                    Name = x.Name,
                    Make = x.Make,
                    Pack = x.Pack,
                    Rate = x.Rate,
                    Gst = x.Gst ?? 0,
                    Hsn = x.Hsn,
                    Msl = x.Msl,
                    CategoryName = x.Category.Name,
                    Unit = x.Unit == null ? null : x.Unit.Name,
                    Active = (YesNo)x.Active
                }).ToList();

            return Json(new PaginatedResponse<ProductVM>
            {
                Items = items,
                TotalItems = totalProducts,
                TotalPages = (int)Math.Ceiling((double)totalProducts / filter.PageSize),
                CurrentPage = filter.PageNumber
            });
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
            ViewBag.Units = await _lookupService.GetUnitsAsync();

            ProductVM productVM = new();
            if (id == null) return View(productVM);

            var productData = _productService.Get(x => x.Id == id, "Category,Unit");
            if (productData == null)
            {
                return NotFound();
            }

            productVM = new()
            {
                Type = ProductTypes.Product,
                Name = productData.Name,
                Make = productData.Make,
                Pack = productData.Pack,
                Rate = productData.Rate,
                Gst = productData.Gst ?? 0,
                Hsn = productData.Hsn,
                Msl = productData.Msl,
                CategoryId = productData.CategoryId,
                CategoryName = productData.Category.Name,
                UnitId = productData.UnitId,
                Unit = productData.Unit?.Name,
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Guid? id, ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
                ViewBag.Units = await _lookupService.GetUnitsAsync();

                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                TempData["Error"] = string.Join("<br/>", errors);

                return View(productVM);
            }

            try
            {
                var product = new Product()
                {
                    Id = productVM.Id ?? Guid.Empty,
                    Type = (int)ProductTypes.Product,
                    Name = productVM.Name,
                    Make = productVM.Make,
                    Pack = productVM.Pack,
                    Rate = productVM.Rate ?? 0,
                    Gst = productVM.Gst,
                    CategoryId = productVM.CategoryId,
                    UnitId = productVM.UnitId,
                    Hsn = productVM.Hsn,
                    Msl = productVM.Msl,
                    Active = (int)productVM.Active
                };

                if (productVM.Id == null)
                {
                    // create
                    _productService.Add(product);
                    _logger.LogInformation($"New Product '{productVM.Name}' added successfully");
                    TempData["Success"] = "Product saved successfully.";
                }
                else
                {
                    _productService.Update(product);
                    _logger.LogInformation($"Product '{productVM.Name}' updated successfully");
                    TempData["Success"] = "Product updated successfully.";
                }
                ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
                ViewBag.Units = await _lookupService.GetUnitsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Product in the database {ex.Message}");
                TempData["Error"] = "Error saving the Product.";
            }

            return View(productVM);
        }
    }
}
