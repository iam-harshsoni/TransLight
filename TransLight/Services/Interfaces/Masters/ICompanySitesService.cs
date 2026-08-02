using TransLight.DataAccess.Common;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;

namespace TransLight.Services.Interfaces.Masters
{
    public interface ICompanySitesService : IBaseService<CompanySite>
    {
        Task<PaginatedResponse<CompanySitesVM>> GetCompanySitesAsync();
        Task<CompanySitesVM> GetForEditAsync(Guid? id);
        Task<PaginatedResponse<CompanySitesVM>> GetByCompanyId(Guid id);
        Task<ServiceReturn<Guid>> SaveAsync(CompanySitesVM vm);

    }
}
