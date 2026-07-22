using TransLight.DataAccess.ViewModels.Masters;

namespace TransLight.Services.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<CountryVM>> GetCountriesAsync();
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
