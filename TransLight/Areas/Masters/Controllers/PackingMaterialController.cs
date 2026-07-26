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
    public class PackingMaterialController : BaseController
    {
        private readonly ILogger<PackingMaterialController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IProductService _productService;

        public PackingMaterialController(ILogger<PackingMaterialController> logger, IProductService productService, ILookupService lookupService)
        {
            _logger = logger;
            _productService = productService;
            _lookupService = lookupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPackingMaterialsData([FromQuery] PackingMaterialFilter filter)
        {
            var query = _productService.GetAll("Category,Unit").AsQueryable().Where(x => x.Type == (int)ProductTypes.PackingMaterial);

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

            int PackingMaterials = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PackingMaterialVM()
                {
                    Id = x.Id,
                    Type = ProductTypes.PackingMaterial,
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

            return Json(new PaginatedResponse<PackingMaterialVM>
            {
                Items = items,
                TotalItems = PackingMaterials,
                TotalPages = (int)Math.Ceiling((double)PackingMaterials / filter.PageSize),
                CurrentPage = filter.PageNumber
            });
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
            ViewBag.Units = await _lookupService.GetUnitsAsync();

            PackingMaterialVM packingMaterialVM = new();
            if (id == null) return View(packingMaterialVM);

            var packingMaterialData = _productService.Get(x => x.Id == id, "Category,Unit");
            if (packingMaterialData == null)
            {
                return NotFound();
            }

            packingMaterialVM = new()
            {
                Type = ProductTypes.PackingMaterial,
                Name = packingMaterialData.Name,
                Make = packingMaterialData.Make,
                Pack = packingMaterialData.Pack,
                Rate = packingMaterialData.Rate,
                Gst = packingMaterialData.Gst ?? 0,
                Hsn = packingMaterialData.Hsn,
                Msl = packingMaterialData.Msl,
                CategoryId = packingMaterialData.CategoryId,
                CategoryName = packingMaterialData.Category.Name,
                UnitId = packingMaterialData.UnitId,
                Unit = packingMaterialData.Unit?.Name,
            };

            return View(packingMaterialVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Guid? id, PackingMaterialVM packingMaterialVM)
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

                return View(packingMaterialVM);
            }

            try
            {
                var packingMaterial = new Product()
                {
                    Id = packingMaterialVM.Id ?? Guid.Empty,
                    Type = (int)ProductTypes.PackingMaterial,
                    Name = packingMaterialVM.Name,
                    Make = packingMaterialVM.Make,
                    Pack = packingMaterialVM.Pack,
                    Rate = packingMaterialVM.Rate ?? 0,
                    Gst = packingMaterialVM.Gst,
                    CategoryId = packingMaterialVM.CategoryId,
                    UnitId = packingMaterialVM.UnitId,
                    Hsn = packingMaterialVM.Hsn,
                    Msl = packingMaterialVM.Msl,
                    Active = (int)packingMaterialVM.Active
                };

                if (packingMaterialVM.Id == null)
                {
                    // create
                    _productService.Add(packingMaterial);
                    _logger.LogInformation($"New Packing Material '{packingMaterialVM.Name}' added successfully");
                    TempData["Success"] = "Packing Material saved successfully.";
                }
                else
                {
                    _productService.Update(packingMaterial);
                    _logger.LogInformation($"Packing Material '{packingMaterialVM.Name}' updated successfully");
                    TempData["Success"] = "Packing Material updated successfully.";
                }
                ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
                ViewBag.Units = await _lookupService.GetUnitsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Packing Material in the database {ex.Message}");
                TempData["Error"] = "Error saving the Packing Material.";
            }

            return View(packingMaterialVM);
        }
    }
}
