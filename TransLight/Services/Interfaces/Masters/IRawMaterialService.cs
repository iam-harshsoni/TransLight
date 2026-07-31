using TransLight.DataAccess.Common;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IRawMaterialService : IBaseService<Product>
    {
        Task<PaginatedResponse<RawMaterialVM>> GetProductAsync(RawMaterialFilter filter);
        Task<RawMaterialVM> GetForEditAsync(Guid? id);
        Task<ServiceReturn<Guid>> SaveAsync(RawMaterialVM vm);
    }
}
