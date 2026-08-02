using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;

namespace TransLight.DataAccess.ViewModels.Dashboard
{
    public class CompanySelectorVM
    {
        public IEnumerable<Company> Companies { get; set; } = [];

        public string SelectedCompanyId { get; set; } = string.Empty;
    }
}
