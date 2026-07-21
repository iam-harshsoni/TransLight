using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;

        public CountryController(ILogger<CountryController> logger, ICountryService countryService)
        {
            _logger = logger;
            _countryService = countryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCountriesData(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var query = _countryService.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            int totalCounties = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CountryVM()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                });

            return Json(new
            {
                items,
                totalCounties,
                totalPages = (int)Math.Ceiling((double)totalCounties / pageSize),
                currentPage = pageNumber
            });
        }

        public IActionResult Upsert(Guid? id)
        {
            CountryVM countryVM = new();
            if (id == null) return View(countryVM);

            var countryData = _countryService.Get(x => x.Id == id);
            if (countryData == null)
            {
                return NotFound();
            }

            countryVM = new()
            {
                Id = countryData.Id,
                Code = countryData.Code ?? "",
                Name = countryData.Name ?? "",
            };

            return View(countryVM);
        }

        [HttpPost]
        public IActionResult Upsert(Guid? id, CountryVM countryVM)
        {
            if (!ModelState.IsValid)
                return View(countryVM);

            try
            {
                var country = new Country()
                {
                    Code = countryVM.Code,
                    Name = countryVM.Name
                };

                if (countryVM.Id.HasValue)
                    countryVM.Id = countryVM.Id.Value;

                if (countryVM.Id == null)
                {
                    // create
                    _countryService.Add(country);
                    _logger.LogInformation($"New country '{countryVM.Name}' added successfully");
                    TempData["Success"] = "Country saved successfully.";
                }
                else
                {
                    _countryService.Update(country);
                    _logger.LogInformation($"New country '{countryVM.Name}' updated successfully");
                    TempData["Success"] = "Country updated successfully.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Country in the database {ex.Message}");
                TempData["Error"] = "Error saving the Country.";
            }

            return View(countryVM);
        }
    }
}
