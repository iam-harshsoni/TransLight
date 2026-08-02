using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.ViewModels.Dashboard;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.ViewComponents
{
    public class CompanySelectorViewComponent(ICompanyService _companyService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var companies = _companyService.GetAll();

            var vm = new CompanySelectorVM()
            {
                Companies = companies.ToList(),
                SelectedCompanyId = HttpContext.Session.GetString("CompanyId") ?? companies.First().Id.ToString()
            };

            return View(vm);
        }
    }
}
