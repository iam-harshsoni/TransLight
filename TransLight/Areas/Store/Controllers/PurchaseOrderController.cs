using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Filters.Store;
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
    }
}
