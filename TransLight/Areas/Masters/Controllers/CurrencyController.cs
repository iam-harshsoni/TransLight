using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class CurrencyController : BaseController
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCurrenciesData(string? code, string? name, int pageNumber = 1, int pageSize = 10)
        {
            var query = _currencyService.GetAll();

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(x => x.Code != null && x.Code.Contains(code, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name != null && x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            int totalCounties = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CurrencyVM()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

            return Json(new PaginatedResponse<CurrencyVM>
            {
                Items = items,
                TotalItems = totalCounties,
                TotalPages = (int)Math.Ceiling((double)totalCounties / pageSize),
                CurrentPage = pageNumber
            });
        }

        public IActionResult Upsert(Guid? id)
        {
            CurrencyVM currencyVM = new();
            if (id == null) return View(currencyVM);

            var currencyData = _currencyService.Get(x => x.Id == id);
            if (currencyData == null)
                return NotFound();

            currencyVM = new()
            {
                Id = currencyData.Id,
                Code = currencyData.Code ?? "",
                Name = currencyData.Name ?? "",
            };

            return View(currencyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Guid? id, CurrencyVM currencyVM)
        {
            if (!ModelState.IsValid)
                return View(currencyVM);

            try
            {
                var currency = new Currency()
                {
                    Id = currencyVM.Id ?? Guid.Empty,
                    Code = currencyVM.Code,
                    Name = currencyVM.Name
                };

                if (currencyVM.Id == null)
                {
                    // create
                    _currencyService.Add(currency);
                    _currencyService.Save();

                    _logger.LogInformation($"New currency '{currencyVM.Name}' added successfully");
                    TempData["Success"] = "Currency saved successfully.";
                }
                else
                {
                    _currencyService.Update(currency);
                    _currencyService.Save();

                    _logger.LogInformation($"Currency '{currencyVM.Name}' updated successfully");
                    TempData["Success"] = "Currency updated successfully.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Currency in the database {ex.Message}");
                TempData["Error"] = "Error saving the Currency.";
            }

            return View(currencyVM);
        }
    }
}
