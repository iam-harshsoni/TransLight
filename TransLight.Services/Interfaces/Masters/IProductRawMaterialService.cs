using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IProductRawMaterialService : IBaseService<ProductRawMaterial>
    {
        void Update(ProductRawMaterial obj);
    }
}
