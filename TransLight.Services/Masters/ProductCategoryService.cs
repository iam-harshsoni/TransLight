using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters;

public class ProductCategoryService(TransLightContext db) : BaseService<ProductCategory>(db), IProductCategoryService
{
    private TransLightContext _db = db;

    public void Update(ProductCategory obj)
    {
        _db.ProductCategories.Update(obj);
        _db.SaveChanges();
    }
}
