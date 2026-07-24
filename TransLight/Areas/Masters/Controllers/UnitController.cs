using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class UnitController : BaseController
    {
        private readonly ILogger<UnitController> _logger;
        private readonly IUnitService _unitService;

        public UnitController(ILogger<UnitController> logger, IUnitService unitService)
        {
            _logger = logger;
            _unitService = unitService;
        }
        public IActionResult Index(UnitVM unitVM)
        {
            return View(unitVM);
        }

        public IActionResult GetUnitsData(string? code, string? name, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitService.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(x => x.Code != null && x.Code.Contains(code, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name != null && x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            int totalUnits = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UnitVM()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

            return Json(new PaginatedResponse<UnitVM>
            {
                Items = items,
                TotalItems = totalUnits,
                TotalPages = (int)Math.Ceiling((double)totalUnits / pageSize),
                CurrentPage = pageNumber
            });
        }

        [HttpGet]
        public IActionResult Upsert(Guid? id)
        {
            UnitVM unitVM = new();
            if (id == null) return View(unitVM);

            var unitData = _unitService.Get(x => x.Id == id);
            if (unitData == null)
            {
                return NotFound();
            }

            unitVM = new()
            {
                Id = unitData.Id,
                Code = unitData.Code ?? "",
                Name = unitData.Name ?? "",
            };

            return RedirectToAction(nameof(Index), new { unitVM });
        }

        [HttpPost]
        public IActionResult Upsert(Guid? id, UnitVM unitVM)
        {
            if (!ModelState.IsValid)
                return View(unitVM);

            try
            {
                var unit = new Unit()
                {
                    Id = unitVM.Id ?? Guid.Empty,
                    Code = unitVM.Code,
                    Name = unitVM.Name
                };

                if (unitVM.Id == null)
                {
                    // create
                    _unitService.Add(unit);
                    _logger.LogInformation($"New unit '{unitVM.Name}' added successfully");
                    TempData["Success"] = "Unit saved successfully.";
                }
                else
                {
                    _unitService.Update(unit);
                    _logger.LogInformation($"Unit '{unitVM.Name}' updated successfully");
                    TempData["Success"] = "Unit updated successfully.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Unit in the database {ex.Message}");
                TempData["Error"] = "Error saving the Unit.";
            }

            return RedirectToAction(nameof(Index), new { unitVM });
        }
    }
}
