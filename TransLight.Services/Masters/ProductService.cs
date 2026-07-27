using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class ProductService(TransLightContext db) : BaseService<Product>(db), IProductService
    {
        private TransLightContext _db = db;

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
            _db.SaveChanges();
        }
    }
}

