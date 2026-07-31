using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface ICountryService : IBaseService<Country>
    {
        void Update(Country obj);
    }
}
