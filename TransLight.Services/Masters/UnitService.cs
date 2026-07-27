using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class UnitService(TransLightContext db) : BaseService<Unit>(db), IUnitService
    {
        private TransLightContext _db = db;

        public void Update(Unit obj)
        {
            _db.Units.Update(obj);
            _db.SaveChanges();
        }
    }
}
