using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface ICurrencyService : IBaseService<Currency>
    {
        void Update(Currency obj);
    }
}
