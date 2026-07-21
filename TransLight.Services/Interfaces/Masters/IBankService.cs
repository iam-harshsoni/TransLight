using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IBankService : IBaseService<Bank>
    {
        void Update(Bank obj);
    }
}
