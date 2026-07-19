using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.ViewModels;
using TransLight.Models.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    [Area("Masters")]
    public class BankController : Controller
    {
        private readonly ILogger<BankController> _logger;
        private readonly IBankService _bankService;

        public BankController(ILogger<BankController> logger, IBankService bankService)
        {
            _logger = logger;
            _bankService = bankService;
        }

        // 1. This renders the empty page shell
        public IActionResult Index()
        {
            return View();
        }

        // 2. This API endpoint handles the live searching and pagination requests from Vue
        [HttpGet]
        public IActionResult GetBanksData(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var query = _bankService.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            int totalItems = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BankVM()
                {
                    Id = x.Id,
                    Name = x.Name ?? ""
                })
                .ToList();

            return Json(new
            {
                items,
                totalItems,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                currentPage = pageNumber
            });
        }

        public IActionResult Upsert(Guid? id)
        {
            BankVM bankVM = new();
            if (id == null)
                return View(bankVM);

            var bankData = _bankService.Get(x => x.Id == id);
            if (bankData == null)
            {
                return NotFound();
            }

            bankVM = new()
            {
                Id = bankData.Id,
                Name = bankData.Name ?? "",
            };

            return View(bankVM);
        }

        [HttpPost]
        public IActionResult Upsert(Guid? id, BankVM bankVM)
        {
            if (!ModelState.IsValid)
                return View(bankVM);

            try
            {
                var bank = new Bank
                {
                    Name = bankVM.Name
                };

                if (bankVM.Id.HasValue) bank.Id = bankVM.Id.Value;

                if (bankVM.Id == null)
                {
                    _bankService.Add(bank);
                    _logger.LogInformation($"New bank '{bankVM.Name}' added successfully");
                    TempData["Success"] = "Bank saved successfully.";
                }
                else
                {
                    _bankService.Update(bank);
                    _logger.LogInformation($"Bank '{bankVM.Name}' Updated Successfully");
                    TempData["Success"] = "Bank Updated successfully.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Bank in the database {ex.Message}");
                TempData["Error"] = "Error saving the Bank.";
            }

            return View(bankVM);
        }
    }
}
