using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Data;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;
using TransLight.Services.Interfaces.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Services.Masters
{
    public class CompanySiteService(TransLightContext db) : BaseService<CompanySite>(db), ICompanySitesService
    {
        private TransLightContext _db = db;

        public async Task<PaginatedResponse<CompanySitesVM>> GetCompanySitesAsync()
        {
            var query = _db.CompanySites.AsNoTracking().Where(x => x.Active == (int)YesNo.Yes);

            int totalCs = query.Count();

            var items = query
                .Select(x => new CompanySitesVM()
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code,
                    Name = x.Name,
                    Address = x.Address,
                    City = x.City,
                    Pincode = x.Pincode,
                    Contact = x.Contact,
                    Email = x.Email,
                    GstNo = x.GstNo,
                    LutNo = x.LutNo,
                    EinvoiceUsername = x.EinvoiceUsername,
                    EwayUsername = x.EwayUsername,
                    EwayPassword = x.EwayPassword,
                    Active = (YesNo)x.Active
                }).ToList();

            return new PaginatedResponse<CompanySitesVM>
            {
                Items = items,
                TotalItems = totalCs,
                TotalPages = 1,
                CurrentPage = 1
            };
        }

        public async Task<PaginatedResponse<CompanySitesVM>> GetByCompanyId(Guid id)
        {
            var query = _db.CompanySites.AsNoTracking().Where(x => x.CompanyId == id);

            int totalCs = await query.CountAsync();

            var items = await query
                .Select(x => new CompanySitesVM()
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code.Trim(),
                    Name = x.Name,
                    Address = x.Address,
                    City = x.City,
                    Pincode = x.Pincode,
                    Contact = x.Contact,
                    Email = x.Email,
                    GstNo = x.GstNo,
                    LutNo = x.LutNo,
                    EinvoiceUsername = x.EinvoiceUsername,
                    EwayUsername = x.EwayUsername,
                    EwayPassword = x.EwayPassword,
                    Active = (YesNo)x.Active
                }).ToListAsync();

            return new PaginatedResponse<CompanySitesVM>
            {
                Items = items,
                TotalItems = totalCs,
                TotalPages = 1,
                CurrentPage = 1
            };
        }

        public async Task<CompanySitesVM> GetForEditAsync(Guid? id)
        {
            if (id == null)
                return new CompanySitesVM();

            var csData = _db.CompanySites
                .AsNoTracking()
                .Where(x => x.Id == id).FirstOrDefault();

            if (csData == null)
                return new CompanySitesVM();

            var vm = new CompanySitesVM()
            {
                Id = csData.Id,
                CompanyId = csData.CompanyId,
                Code = csData.Code,
                Name = csData.Name,
                Address = csData.Address,
                City = csData.City,
                Pincode = csData.Pincode,
                Contact = csData.Contact,
                Email = csData.Email,
                GstNo = csData.GstNo,
                LutNo = csData.LutNo,
                EinvoiceUsername = csData.EinvoiceUsername,
                EwayUsername = csData.EwayUsername,
                EwayPassword = csData.EwayPassword,
                Active = (YesNo)csData.Active,
            };

            return vm;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(CompanySitesVM vm)
        {
            CompanySite cs;
            bool isUpdate = vm.Id.HasValue && vm.Id.Value != Guid.Empty;

            if (isUpdate)
            {
                cs = await _db.CompanySites.FindAsync(vm.Id.Value);
                if (cs == null)
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
                cs = new CompanySite()
                {
                    Id = Guid.NewGuid()
                };
            }

            cs.CompanyId = vm.CompanyId;
            cs.Code = vm.Code;
            cs.Name = vm.Name;
            cs.Address = vm.Address;
            cs.City = vm.City;
            cs.Pincode = vm.Pincode;
            cs.Contact = vm.Contact;
            cs.Email = vm.Email;
            cs.GstNo = vm.GstNo;
            cs.LutNo = vm.LutNo;
            cs.EinvoiceUsername = vm.EinvoiceUsername;
            cs.EwayUsername = vm.EwayUsername;
            cs.EwayPassword = vm.EwayPassword;
            cs.Active = (int)vm.Active;

            try
            {
                if (!isUpdate)
                {
                    _db.CompanySites.Add(cs);
                }
                else
                {
                    _db.CompanySites.Update(cs);
                }

                await _db.SaveChangesAsync();

                return new ServiceReturn<Guid>
                {
                    Success = true,
                    Message = isUpdate ? "Company Site updated successfully." : "Company Site created successfully.",
                    Data = cs.Id
                };
            }
            catch (Exception ex)
            {
                return new ServiceReturn<Guid>
                {
                    Success = false,
                    Message = "Something went wrong during file processing.",
                    Data = cs.Id
                };
            }
        }
    }
}
