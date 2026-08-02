using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.Masters.Controllers
{
    public class CompanySiteController : BaseController
    {
        private readonly ILogger<CompanySiteController> _logger;
        private readonly ICompanySitesService _companySitesService;

        public CompanySiteController(ILogger<CompanySiteController> logger,
            ICompanySitesService companySitesService)
        {
            _logger = logger;
            _companySitesService = companySitesService;
        }

        public async Task<IActionResult> GetByCompanyId([FromQuery] Guid id)
        {
            var result = await _companySitesService.GetByCompanyId(id);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert([FromBody] CompanySitesVM vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                //TempData["Error"] = string.Join("<br/>", errors);

                return BadRequest(new { message = string.Join(" | ", errors) });
            }

            var result = await _companySitesService.SaveAsync(vm);

            if (!result.Success)
            {
                //TempData["Error"] = result.Message;

                return BadRequest(new { message = result.Message });
            }

            //TempData["Success"] = result.Message;

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
