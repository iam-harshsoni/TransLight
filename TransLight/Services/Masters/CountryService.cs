using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class CountryService(TransLightContext db) : BaseService<Country>(db), ICountryService
    {
        private TransLightContext _db = db;

        public void Update(Country obj)
        {
            _db.Countries.Update(obj);
            _db.SaveChanges();
        }
    }
}
