using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IProductCategoryService : IBaseService<ProductCategory>
    {
        void Update(ProductCategory obj);
    }
}
