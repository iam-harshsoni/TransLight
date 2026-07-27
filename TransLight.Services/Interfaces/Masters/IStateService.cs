using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IStateService : IBaseService<State>
    {
        void Update(State obj);
    }
}
