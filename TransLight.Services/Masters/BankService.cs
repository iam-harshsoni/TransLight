using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class BankService(TransLightContext db) : BaseService<Bank>(db), IBankService
    {
        private TransLightContext _db = db;

        public void Update(Bank obj)
        {
            _db.Banks.Update(obj);
            _db.SaveChanges();
        }
    }
}
