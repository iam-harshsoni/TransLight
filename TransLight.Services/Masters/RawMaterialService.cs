using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class RawMaterialService(TransLightContext db) : BaseService<RawMaterial>(db), IRawMaterialService
    {
        private TransLightContext _db = db;

        public void Update(RawMaterial obj)
        {
            _db.RawMaterials.Update(obj);
            _db.SaveChanges();
        }
    }
}
