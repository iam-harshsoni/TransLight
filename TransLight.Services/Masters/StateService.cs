using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class StateService(TransLightContext db) : BaseService<State>(db), IStateService
    {
        private TransLightContext _db = db;

        public void Update(State obj)
        {
            _db.States.Update(obj);
            _db.SaveChanges();
        }
    }
}
