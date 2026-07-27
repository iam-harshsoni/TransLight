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
        private readonly IProductRawMaterialService _productRawMaterialService;

        public ProductController(ILogger<ProductController> logger,
            IProductService productService,
            IProductRawMaterialService productRawMaterialService,
            ILookupService lookupService)
        {
            _logger = logger;
            _productService = productService;
            _lookupService = lookupService;
            _productRawMaterialService = productRawMaterialService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProductsData([FromQuery] ProductFilter filter)
        {
            var query = _productService.GetAll("Category,Unit").Where(x => x.Type == (int)ProductTypes.Product);

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
            await LookUps();

            ProductVM productVM = new();
            if (id == null) return View(productVM);

            var productData = _productService.Get(x => x.Id == id, "Category,Unit");
            if (productData == null)
            {
                return NotFound();
            }

            productVM = new()
            {
                Id = productData.Id,
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

                RawMaterials = _productRawMaterialService.GetAll()
                        .Where(x => x.ProductId == id && x.Type == (int)ProductTypes.RawMaterial)
                        .Select(x => new ProduceRawMaterialsVM
                        {
                            Id = x.Id,
                            ProductId = x.ProductId,
                            RawMaterialId = x.RawMaterialId,
                            RawMaterialName = x.RawMaterial.Name,
                            UnitId = x.UnitId,
                            UnitName = x.Unit != null ? x.Unit.Name : "",
                            Qty = x.Qty,
                            Type = (ProductTypes)x.Type,
                        })
                        .ToList(),

                PackingMaterials = _productRawMaterialService.GetAll()
                        .Where(x => x.ProductId == id && x.Type == (int)ProductTypes.PackingMaterial)
                        .Select(x => new ProduceRawMaterialsVM
                        {
                            Id = x.Id,
                            ProductId = x.ProductId,
                            RawMaterialId = x.RawMaterialId,
                            RawMaterialName = x.RawMaterial.Name,
                            UnitId = x.UnitId,
                            UnitName = x.Unit != null ? x.Unit.Name : "",
                            Qty = x.Qty,
                            Type = (ProductTypes)x.Type,
                        })
                        .ToList(),
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                await LookUps();

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
                    _productService.Save();
                    _logger.LogInformation($"New Product '{productVM.Name}' added successfully");
                    TempData["Success"] = "Product saved successfully.";
                }
                else
                {
                    _productService.Update(product);

                    #region Add/Update RawMaterials & ProduceMaterials
                    // Remove All existing RawMaterials and PackingMaterials
                    var existingMappings = _productRawMaterialService
                        .GetAll()
                        .Where(x => x.ProductId == product.Id)
                        .ToList();

                    if (existingMappings.Any())
                        _productRawMaterialService.RemoveRange(existingMappings);

                    // Update RawMaterials
                    if (productVM.RawMaterials.Count > 0)
                    {
                        foreach (var item in productVM.RawMaterials)
                        {
                            if (!item.IsSelected)
                            {
                                var rawMaterials = new ProductRawMaterial
                                {
                                    ProductId = item.ProductId,
                                    RawMaterialId = item.RawMaterialId,
                                    UnitId = item.UnitId,
                                    Qty = item.Qty,
                                    Type = (int)ProductTypes.RawMaterial
                                };
                                _productRawMaterialService.Add(rawMaterials);
                            }
                        }
                    }

                    // Update PackingMaterials
                    if (productVM.PackingMaterials.Count > 0)
                    {
                        foreach (var item in productVM.PackingMaterials)
                        {
                            if (!item.IsSelected)
                            {
                                var packingMaterials = new ProductRawMaterial
                                {
                                    ProductId = item.ProductId,
                                    RawMaterialId = item.RawMaterialId, // packingMaterial Id
                                    UnitId = item.UnitId,
                                    Qty = item.Qty,
                                    Type = (int)ProductTypes.PackingMaterial
                                };
                                _productRawMaterialService.Add(packingMaterials);
                            }
                        }
                    }
                    #endregion

                    _productService.Save();
                    _logger.LogInformation($"Product '{productVM.Name}' updated successfully");
                    TempData["Success"] = "Product updated successfully.";
                }

                await LookUps();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Product in the database {ex.Message}");
                TempData["Error"] = "Error saving the Product.";
            }

            return RedirectToAction(nameof(Upsert), new { id = productVM.Id });
        }

        private async Task LookUps()
        {
            ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
            ViewBag.Units = await _lookupService.GetUnitsAsync();
            ViewBag.RawMaterials = await _lookupService.GetProductsByTypeAsync(ProductTypes.RawMaterial);
            ViewBag.PackingMaterials = await _lookupService.GetProductsByTypeAsync(ProductTypes.PackingMaterial);
        }
    }
}
