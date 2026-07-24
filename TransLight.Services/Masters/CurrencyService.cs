using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class CurrencyService(TransLightContext db) : BaseService<Currency>(db), ICurrencyService
    {
        private TransLightContext _db = db;

        public void Update(Currency obj)
        {
            _db.Currencies.Update(obj);
            _db.SaveChanges();
        }
    }
}
