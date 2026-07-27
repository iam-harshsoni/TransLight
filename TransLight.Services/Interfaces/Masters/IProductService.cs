using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IProductService : IBaseService<Product>
    {
        void Update(Product obj);
    }
}
