using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Data;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class CompanyService(TransLightContext db) : BaseService<Company>(db), ICompanyService
    {
        private TransLightContext _db = db;

        public async Task<PaginatedResponse<CompanyVM>> GetCompanyAsync(CompanyFilters filter)
        {
            var query = _db.Companies.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Code))
                query = query.Where(x => x.Code != null && x.Code.ToLower().Contains(filter.Code.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name != null && x.Name.ToLower().Contains(filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Address))
                query = query.Where(x => x.Address != null && x.Address.ToLower().Contains(filter.Address.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Contact))
                query = query.Where(x => x.Contact != null && x.Contact.ToLower().Contains(filter.Contact.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(x => x.Email != null && x.Email.ToLower().Contains(filter.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.GstNo))
                query = query.Where(x => x.GstNo != null && x.GstNo.ToLower().Contains(filter.GstNo.ToLower()));

            int totalCompanies = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new CompanyVM()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Address = x.Address,
                    Contact = x.Contact,
                    Email = x.Email,
                    GstNo = x.GstNo,
                }).ToList();

            return new PaginatedResponse<CompanyVM>
            {
                Items = items,
                TotalItems = totalCompanies,
                TotalPages = (int)Math.Ceiling((double)totalCompanies / filter.PageSize),
                CurrentPage = filter.PageNumber
            };
        }

        public async Task<CompanyVM> GetForEditAsync(Guid? id)
        {
            if (id == null)
                return new CompanyVM();

            var companyData = _db.Companies
                .AsNoTracking()
                .Where(x => x.Id == id).FirstOrDefault();

            if (companyData == null)
                return new CompanyVM();

            var companyVM = new CompanyVM()
            {
                Id = companyData.Id,
                Code = companyData.Code,
                Name = companyData.Name,
                Address = companyData.Address,
                City = companyData.City,
                Pincode = companyData.Pincode,
                Contact = companyData.Contact,
                Email = companyData.Email,
                PanNo = companyData.PanNo,
                TanNo = companyData.TanNo,
                CinNo = companyData.CinNo,
                GstNo = companyData.GstNo,
                MsmeNo = companyData.MsmeNo,
                Remarks = companyData.Remarks,
                Logo = companyData.Logo,
                Signature = companyData.Signature,
            };

            return companyVM;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(CompanyVM vm)
        {
            var company = new Company()
            {
                Id = vm.Id ?? Guid.Empty,
                Code = vm.Code,
                Name = vm.Name,
                Address = vm.Address,
                City = vm.City,
                Pincode = vm.Pincode,
                Contact = vm.Contact,
                Email = vm.Email,
                PanNo = vm.PanNo,
                TanNo = vm.TanNo,
                CinNo = vm.CinNo,
                GstNo = vm.GstNo,
                MsmeNo = vm.MsmeNo,
                Remarks = vm.Remarks,
            };

            if (vm.Id == null)
            {
                // create
                _db.Companies.Add(company);
            }
            else
            {
                _db.Companies.Update(company);
            }

            await _db.SaveChangesAsync();

            return new ServiceReturn<Guid>
            {
                Success = true,
                Message = vm.Id == null
                    ? "Company created successfully."
                    : "Company updated successfully.",
                Data = company.Id
            };
        }

    }
}
