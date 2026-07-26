using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services
{
    public class LookupService(
        ICountryService countryService,
        IProductCategoryService productCategoryService,
        IUnitService unitService
        ) : ILookupService
    {
        public async Task<IEnumerable<CountryVM>> GetCountriesAsync()
        {
            var result = countryService.GetAll().Take(500).Select(x => new CountryVM
            {
                Id = x.Id,
                Name = x.Name.ToUpper()
            });
            return result ?? [];
        }

        public async Task<IEnumerable<ProductCategoryVM>> GetProductCategoriesAsync()
        {
            var result = productCategoryService.GetAll().Take(500).Select(x => new ProductCategoryVM
            {
                Id = x.Id,
                Name = x.Name.ToUpper()
            });
            return result ?? [];
        }

        public async Task<IEnumerable<UnitVM>> GetUnitsAsync()
        {
            var result = unitService.GetAll().Take(500).Select(x => new UnitVM
            {
                Id = x.Id,
                Name = x.Code.ToUpper()
            });
            return result ?? [];
        }
    }
}
