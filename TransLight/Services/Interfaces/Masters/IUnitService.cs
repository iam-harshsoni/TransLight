using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IUnitService : IBaseService<Unit>
    {
        void Update(Unit obj);
    }
}
