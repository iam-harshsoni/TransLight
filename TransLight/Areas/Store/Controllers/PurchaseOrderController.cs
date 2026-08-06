using Microsoft.AspNetCore.Mvc;
using TransLight.Common;
using TransLight.DataAccess.Filters.Store;
using TransLight.DataAccess.ViewModels.Store;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Store;

namespace TransLight.Areas.Store.Controllers
{
    public class PurchaseOrderController : BaseController
    {
        private readonly ILogger<PurchaseOrderController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(ILogger<PurchaseOrderController> logger,
            IPurchaseOrderService purchaseOrderService,
            ILookupService lookupService)
        {
            _logger = logger;
            _lookupService = lookupService;
            _purchaseOrderService = purchaseOrderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPurchaseOrdersData([FromQuery] PurchaseOrderFilters filter)
        {
            var result = await _purchaseOrderService.GetPurchaseOrdersAsync(filter);
            return Json(result);
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            await LookUps();

            var vm = await _purchaseOrderService.GetForEditAsync(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PurchaseOrderVM vm)
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

            vm.CompanyId = HttpContext.Session.GetCompanyId() ?? Guid.Empty;
            var result = await _purchaseOrderService.SaveAsync(vm);

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
            ViewBag.Currencies = await _lookupService.GetCurrenciesAsync();
            ViewBag.CompanySites = await _lookupService.GetCompanySitesByCompanyIdAsync(HttpContext.Session.GetCompanyId() ?? Guid.Empty);
            ViewBag.Products = await _lookupService.GetProductsByTypeAsync();
            ViewBag.Units = await _lookupService.GetUnitsAsync();
            // Party Sites
        }
    }
}
