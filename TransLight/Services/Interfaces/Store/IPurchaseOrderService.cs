using TransLight.DataAccess.Common;
using TransLight.DataAccess.Filters.Store;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Store;

namespace TransLight.Services.Interfaces.Store
{
    public interface IPurchaseOrderService : IBaseService<Transaction>
    {
        Task<PaginatedResponse<PurchaseOrderVM>> GetPurchaseOrdersAsync(PurchaseOrderFilters filter, string? includeProperties = null);
        Task<PurchaseOrderVM> GetForEditAsync(Guid? id);
        //Task<ServiceReturn<Guid>> SaveAsync(PurchaseOrderVM vm);
    }
}
