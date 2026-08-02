using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService _companyService;
        private readonly ICompanySitesService _companySitesService;

        public CompanyController(ILogger<CompanyController> logger,
           ICompanyService companyService,
            ICompanySitesService companySitesService)
        {
            _logger = logger;
            _companyService = companyService;
            _companySitesService = companySitesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetCompaniesData([FromQuery] CompanyFilters filter)
        {
            var result = await _companyService.GetCompanyAsync(filter);
            return Json(result);
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            var vm = await _companyService.GetForEditAsync(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompanyVM vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                TempData["Error"] = string.Join("<br/>", errors);

                return View(vm);
            }

            var result = await _companyService.SaveAsync(vm);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Upsert), new { id = result.Data });
        }
    }
}
