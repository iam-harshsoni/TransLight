using TransLight.DataAccess.Common;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;

namespace TransLight.Services.Interfaces.Masters
{
    public interface ICompanyService : IBaseService<Company>
    {
        Task<PaginatedResponse<CompanyVM>> GetCompanyAsync(CompanyFilters filters);
        Task<CompanyVM> GetForEditAsync(Guid? id);
        Task<ServiceReturn<Guid>> SaveAsync(CompanyVM vm);
    }
}
