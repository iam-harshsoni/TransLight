using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services
{
    public class LookupService(ICountryService countryService) : ILookupService
    {
        public async Task<IEnumerable<CountryVM>> GetCountriesAsync()
        {
            var result = countryService.GetAll().Take(500).Select(x => new CountryVM
            {
                Id = x.Id,
                Name = x.Name
            });
            return result ?? [];
        }
    }
}
