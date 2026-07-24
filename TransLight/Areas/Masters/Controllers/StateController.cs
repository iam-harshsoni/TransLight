using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Areas.Masters.Controllers
{
    public class StateController : BaseController
    {
        private readonly ILogger<StateController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IStateService _stateService;

        public StateController(ILogger<StateController> logger, IStateService stateService, ILookupService lookupService)
        {
            _logger = logger;
            _stateService = stateService;
            _lookupService = lookupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetStatesData(string? gst, string? code, string? country, string? name, int union_t = -1, int pageNumber = 1, int pageSize = 10)
        {
            var query = _stateService.GetAll("Country").AsQueryable();

            if (!string.IsNullOrWhiteSpace(gst))
                query = query.Where(x => x.Gst != null && x.Code.Contains(gst, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(x => x.Code != null && x.Code.Contains(code, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name != null && x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (union_t > -1)
                query = query.Where(x => x.UnionTerritory == union_t);

            //if (!string.IsNullOrWhiteSpace(country))
            //    query = query.Where(x => x.Country.Name != null && x.Country.Name.Contains(country, StringComparison.OrdinalIgnoreCase));

            //if (!string.IsNullOrWhiteSpace(union_t) && Enum.TryParse<YesNo>(union_t, true, out var unionTerritory))
            //{
            //    query = query.Where(x => x.UnionTerritory == (int)unionTerritory);
            //}


            int totalStates = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new StateVM()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Gst = x.Gst,
                    UnionTerritory = ((YesNo)x.UnionTerritory),
                    CountryName = x.Country.Name
                }).ToList();

            return Json(new PaginatedResponse<StateVM>
            {
                Items = items,
                TotalItems = totalStates,
                TotalPages = (int)Math.Ceiling((double)totalStates / pageSize),
                CurrentPage = pageNumber
            });
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            ViewBag.Countries = await _lookupService.GetCountriesAsync();

            StateVM stateVM = new();
            if (id == null) return View(stateVM);

            var stateData = _stateService.Get(x => x.Id == id, "Country");
            if (stateData == null)
            {
                return NotFound();
            }

            stateVM = new()
            {
                Gst = stateData.Gst,
                Code = stateData.Code,
                Name = stateData.Name,
                CountryId = stateData.CountryId,
                UnionTerritory = (YesNo)stateData.UnionTerritory
            };

            return View(stateVM);
        }

        [HttpPost]
        public IActionResult Upsert(Guid? id, StateVM stateVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _lookupService.GetCountriesAsync();
                return View(stateVM);
            }

            try
            {
                var state = new State()
                {
                    Id = stateVM.Id ?? Guid.Empty,
                    Gst = stateVM.Gst,
                    Code = stateVM.Code,
                    Name = stateVM.Name,
                    CountryId = stateVM.CountryId,
                    UnionTerritory = (int)stateVM.UnionTerritory
                };

                if (stateVM.Id == null)
                {
                    // create
                    _stateService.Add(state);
                    _logger.LogInformation($"New state '{stateVM.Name}' added successfully");
                    TempData["Success"] = "State saved successfully.";
                }
                else
                {
                    _stateService.Update(state);
                    _logger.LogInformation($"State '{stateVM.Name}' updated successfully");
                    TempData["Success"] = "State updated successfully.";
                }
                ViewBag.Countries = _lookupService.GetCountriesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the State in the database {ex.Message}");
                TempData["Error"] = "Error saving the State.";
            }

            return View(stateVM);
        }
    }
}
