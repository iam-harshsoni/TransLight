using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Filters.Masters;
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

        public async Task<IActionResult> GetProductsData([FromQuery] ProductFilter filter)
        {
            var result = await _productService.GetProductAsync(filter);
            return Json(result);
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            await LookUps();

            var vm = await _productService.GetForEditAsync(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM vm)
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

            var result = await _productService.SaveAsync(vm);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                await LookUps();

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Upsert),
                new { id = result.Data });

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
