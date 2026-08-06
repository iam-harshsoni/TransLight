using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Services
{
    public class LookupService(
        ICountryService countryService,
        IProductService productService,
        IProductCategoryService productCategoryService,
        IUnitService unitService,
        ICurrencyService currencyService,
        ICompanySitesService companySitesService
        ) : ILookupService
    {
        public async Task<IEnumerable<CountryVM>> GetCountriesAsync()
        {
            var result = countryService.GetAll().Take(500).Select(x => new CountryVM
            {
                Id = x.Id,
                Name = x.Name.ToUpper()
            });
            return result.ToList() ?? [];
        }

        public async Task<IEnumerable<ProductCategoryVM>> GetProductCategoriesAsync()
        {
            var result = productCategoryService.GetAll().Take(500).Select(x => new ProductCategoryVM
            {
                Id = x.Id,
                Name = x.Name.ToUpper()
            });
            return result.ToList() ?? [];
        }

        public async Task<IEnumerable<UnitVM>> GetUnitsAsync()
        {
            var result = unitService.GetAll().Take(500).Select(x => new UnitVM
            {
                Id = x.Id,
                Name = x.Code.ToUpper()
            });
            return result.ToList() ?? [];
        }

        public async Task<IEnumerable<ProductVM>> GetProductsByTypeAsync(ProductTypes? type = 0)
        {
            var query = productService.GetAll();

            if (type > 0)
                query = query.Where(x => x.Type == (int)type);

            var result = query.Take(500).Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name.ToUpper()
            });
            return result.ToList() ?? [];
        }

        public async Task<IEnumerable<CurrencyVM>> GetCurrenciesAsync()
        {
            var result = currencyService.GetAll().Take(500).Select(x => new CurrencyVM
            {
                Id = x.Id,
                Name = x.Code.ToUpper()
            });
            return result.ToList() ?? [];
        }

        public async Task<IEnumerable<CompanySitesVM>> GetCompanySitesAsync()
        {
            var result = companySitesService.GetAll().Take(500).Select(x => new CompanySitesVM
            {
                Id = x.Id,
                Name = string.Concat(x.Name.Trim().ToString(), x.Code.Trim().ToUpper())
            });
            return result.ToList() ?? [];
        }

        public async Task<IEnumerable<CompanySitesVM>> GetCompanySitesByCompanyIdAsync(Guid id)
        {
            var result = companySitesService.GetAll().Where(x => x.CompanyId == id).Take(500).Select(x => new CompanySitesVM
            {
                Id = x.Id,
                Name = x.Name.Trim().ToString()
            });
            return result.ToList() ?? [];
        }
    }
}
