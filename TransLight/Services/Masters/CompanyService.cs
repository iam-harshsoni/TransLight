using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Data;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;
using TransLight.Services.Interfaces;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Services.Masters
{
    public class CompanyService(TransLightContext db, IStorageService storageService) : BaseService<Company>(db), ICompanyService
    {
        private TransLightContext _db = db;
        private IStorageService _storageService = storageService;
        private const string StorageFolder = "company-documents";

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
                    Code = x.Code.Trim(),
                    Name = x.Name.Trim(),
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
                Code = companyData.Code.Trim(),
                Name = companyData.Name.Trim(),
                Address = companyData.Address.Trim(),
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
                LogoUrl = companyData.Logo?.Length > 0 ? _storageService.GetUrl($"uploads/{StorageFolder}/{companyData.Logo}") : "",
                SignatureUrl = companyData.Signature?.Length > 0 ? _storageService.GetUrl($"uploads/{StorageFolder}/{companyData.Signature}") : "",
                StampUrl = companyData.Stamp?.Length > 0 ? _storageService.GetUrl($"uploads/{StorageFolder}/{companyData.Stamp}") : "",
            };

            return companyVM;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(CompanyVM vm)
        {
            Company company;
            bool isUpdate = vm.Id.HasValue && vm.Id.Value != Guid.Empty;

            if (isUpdate)
            {
                // 1. Fetch the existing company so we don't lose the current Logo/Signature/Stamp paths
                company = await _db.Companies.FindAsync(vm.Id.Value);
                if (company == null)
                {
                    return new ServiceReturn<Guid>
                    {
                        Success = false,
                        Message = "Company not found"
                    };
                }
            }
            else
            {
                // 1. Create a fresh company object for new records
                company = new Company()
                {
                    Id = Guid.NewGuid() // Or leave empty if DB generates it
                };
            }

            // 2. Map all standard properties
            company.Code = vm.Code;
            company.Name = vm.Name;
            company.Address = vm.Address;
            company.City = vm.City;
            company.Pincode = vm.Pincode;
            company.Contact = vm.Contact;
            company.Email = vm.Email;
            company.PanNo = vm.PanNo;
            company.TanNo = vm.TanNo;
            company.CinNo = vm.CinNo;
            company.GstNo = vm.GstNo;
            company.MsmeNo = vm.MsmeNo;
            company.Remarks = vm.Remarks;

            // 3. Handle File Uploads
            try
            {
                if (!isUpdate)
                {
                    // --- CREATE LOGIC ---
                    company.Logo = vm.Logo != null ? await _storageService.SaveAsync(vm.Logo, StorageFolder) : null;
                    company.Signature = vm.Signature != null ? await _storageService.SaveAsync(vm.Signature, StorageFolder) : null;
                    company.Stamp = vm.Stamp != null ? await _storageService.SaveAsync(vm.Stamp, StorageFolder) : null; // Fixed: Stamp is now assigned

                    _db.Companies.Add(company);
                }
                else
                {
                    // --- UPDATE LOGIC ---
                    // Because we fetched 'company' from the DB, 'company.Logo' contains the actual old path string!
                    company.Logo = vm.RemoveLogo
                        ? await DeleteFileAsync(company.Logo)
                        : await UpdateFileAsync(vm.Logo, company.Logo);

                    company.Signature = vm.RemoveSignature
                        ? await DeleteFileAsync(company.Signature)
                        : await UpdateFileAsync(vm.Signature, company.Signature);

                    // Fixed: Added missing Stamp update logic
                    company.Stamp = vm.RemoveStamp
                        ? await DeleteFileAsync(company.Stamp)
                        : await UpdateFileAsync(vm.Stamp, company.Stamp);

                    _db.Companies.Update(company);
                }

                await _db.SaveChangesAsync();

                return new ServiceReturn<Guid>
                {
                    Success = true,
                    Message = isUpdate ? "Company updated successfully." : "Company created successfully.",
                    Data = company.Id
                };
            }
            catch (Exception ex)
            {
                return new ServiceReturn<Guid>
                {
                    Success = false,
                    Message = "Something went wrong during file processing.",
                    Data = company.Id
                };
            }
        }
        private async Task<string?> UpdateFileAsync(IFormFile? file, string? oldPath)
        {
            if (file is null)
                return oldPath;

            var newPath = await _storageService.SaveAsync(file, StorageFolder);

            if (!string.IsNullOrWhiteSpace(oldPath))
            {
                await _storageService.DeleteAsync(oldPath, StorageFolder);
            }

            return newPath;
        }
        private async Task<string?> DeleteFileAsync(string? path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                await _storageService.DeleteAsync(path, StorageFolder);
            return null;
        }

    }
}
