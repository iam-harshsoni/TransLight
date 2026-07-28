using TransLight.DataAccess.Common;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IPackingMaterialService
    {
        Task<PaginatedResponse<PackingMaterialVM>> GetProductAsync(PackingMaterialFilter filter);
        Task<PackingMaterialVM> GetForEditAsync(Guid? id);
        Task<ServiceReturn<Guid>> SaveAsync(PackingMaterialVM vm);
    }
}
