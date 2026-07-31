using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters;

public class ProductRawMaterialService(TransLightContext db) : BaseService<ProductRawMaterial>(db), IProductRawMaterialService
{
    private TransLightContext _db = db;

    public void Update(ProductRawMaterial obj)
    {
        _db.ProductRawMaterials.Update(obj);
        _db.SaveChanges();
    }
}
