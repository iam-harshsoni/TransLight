using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Data;
using TransLight.DataAccess.Filters.Masters;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Common;
using TransLight.Services.Interfaces.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Services.Masters
{
    public class RawMaterialServices(TransLightContext db) : BaseService<Product>(db), IRawMaterialService
    {
        private TransLightContext _db = db;

        public async Task<PaginatedResponse<RawMaterialVM>> GetProductAsync(RawMaterialFilter filter)
        {
            var query = _db.Products.AsNoTracking().Include(x => x.Category).Include(x => x.Unit).Where(x => x.Type == (int)ProductTypes.RawMaterial);

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name != null && x.Name.ToLower().Contains(filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Make))
                query = query.Where(x => x.Make != null && x.Make.ToLower().Contains(filter.Make.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Pack))
                query = query.Where(x => x.Pack != null && x.Pack.ToLower().Contains(filter.Pack.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Hsn))
                query = query.Where(x => x.Hsn != null && x.Hsn.ToLower().Contains(filter.Hsn.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
                query = query.Where(x => x.Category.Name != null && x.Category.Name.ToLower().Contains(filter.CategoryName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.UnitName))
                query = query.Where(x => x.Unit.Name != null && x.Unit.Name.ToLower().Contains(filter.UnitName.ToLower()));

            if (filter.Active > -1)
                query = query.Where(x => x.Active == filter.Active);

            int RawMaterials = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new RawMaterialVM()
                {
                    Id = x.Id,
                    Type = ProductTypes.RawMaterial,
                    Name = x.Name,
                    Make = x.Make,
                    Pack = x.Pack,
                    Rate = x.Rate,
                    Gst = x.Gst ?? 0,
                    Hsn = x.Hsn,
                    Msl = x.Msl,
                    CategoryName = x.Category.Name,
                    Unit = x.Unit == null ? null : x.Unit.Name,
                    Active = (YesNo)x.Active
                }).ToList();

            return new PaginatedResponse<RawMaterialVM>
            {
                Items = items,
                TotalItems = RawMaterials,
                TotalPages = (int)Math.Ceiling((double)RawMaterials / filter.PageSize),
                CurrentPage = filter.PageNumber
            };
        }

        public async Task<RawMaterialVM> GetForEditAsync(Guid? id)
        {
            if (id == null)
                return new RawMaterialVM();

            var rawMaterialData = _db.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Unit)
                .Where(x => x.Id == id).FirstOrDefault();

            if (rawMaterialData == null)
                return new RawMaterialVM();

            var vm = new RawMaterialVM
            {
                Id = rawMaterialData.Id,
                Type = ProductTypes.RawMaterial,
                Name = rawMaterialData.Name,
                Make = rawMaterialData.Make,
                Pack = rawMaterialData.Pack,
                Rate = rawMaterialData.Rate,
                Gst = rawMaterialData.Gst ?? 0,
                Hsn = rawMaterialData.Hsn,
                Msl = rawMaterialData.Msl,
                CategoryId = rawMaterialData.CategoryId,
                CategoryName = rawMaterialData.Category.Name,
                UnitId = rawMaterialData.UnitId,
                Unit = rawMaterialData.Unit?.Name,
            };

            return vm;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(RawMaterialVM vm)
        {
            var rawMaterial = new Product()
            {
                Id = vm.Id ?? Guid.Empty,
                Type = (int)ProductTypes.RawMaterial,
                Name = vm.Name,
                Make = vm.Make,
                Pack = vm.Pack,
                Rate = vm.Rate ?? 0,
                Gst = vm.Gst,
                CategoryId = vm.CategoryId,
                UnitId = vm.UnitId,
                Hsn = vm.Hsn,
                Msl = vm.Msl,
                Active = (int)vm.Active
            };

            if (vm.Id == null)
            {
                // create
                _db.Products.Add(rawMaterial);
            }
            else
            {
                _db.Products.Update(rawMaterial);
            }

            await _db.SaveChangesAsync();

            return new ServiceReturn<Guid>
            {
                Success = true,
                Message = vm.Id == null
                    ? "Raw material created successfully."
                    : "Raw material updated successfully.",
                Data = rawMaterial.Id
            };
        }
    }
}
