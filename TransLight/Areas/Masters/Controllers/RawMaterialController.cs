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
    public class RawMaterialController : BaseController
    {
        private readonly ILogger<RawMaterialController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IRawMaterialService _rawMaterialService;

        public RawMaterialController(ILogger<RawMaterialController> logger, IRawMaterialService rawMaterialService, ILookupService lookupService)
        {
            _logger = logger;
            _rawMaterialService = rawMaterialService;
            _lookupService = lookupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRawMaterialsData([FromQuery] RawMaterialFilter filter)
        {
            var query = _rawMaterialService.GetAll("Category,Unit").AsQueryable();

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

            int totalRawMaterials = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new RawMaterialVM()
                {
                    Id = x.Id,
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

            return Json(new PaginatedResponse<RawMaterialVM>
            {
                Items = items,
                TotalItems = totalRawMaterials,
                TotalPages = (int)Math.Ceiling((double)totalRawMaterials / filter.PageSize),
                CurrentPage = filter.PageNumber
            });
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
            ViewBag.Units = await _lookupService.GetUnitsAsync();

            RawMaterialVM rawMaterialVM = new();
            if (id == null) return View(rawMaterialVM);

            var rawMaterialData = _rawMaterialService.Get(x => x.Id == id, "Category,Unit");
            if (rawMaterialData == null)
            {
                return NotFound();
            }

            rawMaterialVM = new()
            {
                Name = rawMaterialData.Name,
                Make = rawMaterialData.Make,
                Pack = rawMaterialData.Pack,
                Rate = rawMaterialData.Rate,
                Gst = rawMaterialData.Gst ?? 0,
                Hsn = rawMaterialData.Hsn,
                Msl = rawMaterialData.Msl,
                CategoryId = rawMaterialData.CategoryId,
                CategoryName = rawMaterialData.Category.Name,
                UnitId = rawMaterialData.UnitId,
                Unit = rawMaterialData.Unit?.Name,
            };

            return View(rawMaterialVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Guid? id, RawMaterialVM rawMaterialVM)
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

                return View(rawMaterialVM);
            }

            try
            {
                var rawMaterial = new RawMaterial()
                {
                    Id = rawMaterialVM.Id ?? Guid.Empty,
                    Name = rawMaterialVM.Name,
                    Make = rawMaterialVM.Make,
                    Pack = rawMaterialVM.Pack,
                    Rate = rawMaterialVM.Rate ?? 0,
                    Gst = rawMaterialVM.Gst,
                    CategoryId = rawMaterialVM.CategoryId,
                    UnitId = rawMaterialVM.UnitId,
                    Hsn = rawMaterialVM.Hsn,
                    Msl = rawMaterialVM.Msl,
                    Active = (int)rawMaterialVM.Active
                };

                if (rawMaterialVM.Id == null)
                {
                    // create
                    _rawMaterialService.Add(rawMaterial);
                    _logger.LogInformation($"New Raw Material '{rawMaterialVM.Name}' added successfully");
                    TempData["Success"] = "Raw Material saved successfully.";
                }
                else
                {
                    _rawMaterialService.Update(rawMaterial);
                    _logger.LogInformation($"Raw Material '{rawMaterialVM.Name}' updated successfully");
                    TempData["Success"] = "Raw Material updated successfully.";
                }
                ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
                ViewBag.Units = await _lookupService.GetUnitsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Raw Material in the database {ex.Message}");
                TempData["Error"] = "Error saving the Raw Material.";
            }

            return View(rawMaterialVM);
        }
    }
}
