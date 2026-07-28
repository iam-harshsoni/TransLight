using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class PackingMaterialController : BaseController
    {
        private readonly ILogger<PackingMaterialController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IPackingMaterialService _packingMaterialService;

        public PackingMaterialController(ILogger<PackingMaterialController> logger, IPackingMaterialService packingMaterialService, ILookupService lookupService)
        {
            _logger = logger;
            _packingMaterialService = packingMaterialService;
            _lookupService = lookupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPackingMaterialsData([FromQuery] PackingMaterialFilter filter)
        {
            var result = await _packingMaterialService.GetProductAsync(filter);
            return Json(result);
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            await LookUps();

            var vm = await _packingMaterialService.GetForEditAsync(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Guid? id, PackingMaterialVM vm)
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

            var result = await _packingMaterialService.SaveAsync(vm);

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
