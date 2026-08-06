using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Services.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<CountryVM>> GetCountriesAsync();
        Task<IEnumerable<ProductCategoryVM>> GetProductCategoriesAsync();
        Task<IEnumerable<UnitVM>> GetUnitsAsync();
        Task<IEnumerable<ProductVM>> GetProductsByTypeAsync(ProductTypes? type = 0);
        Task<IEnumerable<CurrencyVM>> GetCurrenciesAsync();
        Task<IEnumerable<CompanySitesVM>> GetCompanySitesAsync();
        Task<IEnumerable<CompanySitesVM>> GetCompanySitesByCompanyIdAsync(Guid id);

        //Task<IEnumerable<BankVM>> GetBanksAsync();
        //Task<IEnumerable<StateVM>> GetStatesAsync();
        //Task<IEnumerable<BankBranchVM>> GetBankBranchAsync();
        //Task<IEnumerable<BankAccountVM>> GetBankAccountAsync();
        //Task<IEnumerable<CurrencyVM>> GetCurrencyAsync();
        //Task<IEnumerable<CityVM>> GetCityAsync();
        //Task<IEnumerable<PartyVM>> GetLineAsync();
        //Task<IEnumerable<VesselVM>> GetVesselAsync();
        //Task<IEnumerable<IndianPortVM>> GetIndianPortAsync();
        //Task<IEnumerable<PortVM>> GetPortAsync();
    }
}
