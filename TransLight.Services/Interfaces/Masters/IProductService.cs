using TransLight.DataAccess.Common;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IProductService : IBaseService<Product>
    {
        void Update(Product obj);
        Task<PaginatedResponse<ProductVM>> GetProductAsync(ProductFilter filter);
        Task<ProductVM> GetForEditAsync(Guid? id);
        Task<ServiceReturn<Guid>> SaveAsync(ProductVM vm);
    }
}
