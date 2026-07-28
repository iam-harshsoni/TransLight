using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class RawMaterialController : BaseController
    {
        private readonly ILogger<RawMaterialController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IRawMaterialService _rawMaterialService;

        public RawMaterialController(ILogger<RawMaterialController> logger, IRawMaterialService packingMaterialService, ILookupService lookupService)
        {
            _logger = logger;
            _rawMaterialService = packingMaterialService;
            _lookupService = lookupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetRawMaterialsData([FromQuery] RawMaterialFilter filter)
        {
            var result = await _rawMaterialService.GetProductAsync(filter);
            return Json(result);
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            await LookUps();

            var vm = await _rawMaterialService.GetForEditAsync(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Guid? id, RawMaterialVM vm)
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

                return View(vm);
            }

            var result = await _rawMaterialService.SaveAsync(vm);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                await LookUps();

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Upsert), new { id = result.Data });
        }

        private async Task LookUps()
        {
            ViewBag.Categories = await _lookupService.GetProductCategoriesAsync();
            ViewBag.Units = await _lookupService.GetUnitsAsync();
        }
    }
}
