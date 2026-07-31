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
    public class PackingMaterialServices(TransLightContext db) : BaseService<Product>(db), IPackingMaterialService
    {
        private TransLightContext _db = db;

        public async Task<PaginatedResponse<PackingMaterialVM>> GetProductAsync(PackingMaterialFilter filter)
        {
            var query = _db.Products.AsNoTracking().Include(x => x.Category).Include(x => x.Unit).Where(x => x.Type == (int)ProductTypes.PackingMaterial);

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

            int PackingMaterials = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PackingMaterialVM()
                {
                    Id = x.Id,
                    Type = ProductTypes.PackingMaterial,
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

            return new PaginatedResponse<PackingMaterialVM>
            {
                Items = items,
                TotalItems = PackingMaterials,
                TotalPages = (int)Math.Ceiling((double)PackingMaterials / filter.PageSize),
                CurrentPage = filter.PageNumber
            };
        }

        public async Task<PackingMaterialVM> GetForEditAsync(Guid? id)
        {
            if (id == null)
                return new PackingMaterialVM();

            var packingMaterialData = _db.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Unit)
                .Where(x => x.Id == id).FirstOrDefault();

            if (packingMaterialData == null)
                return new PackingMaterialVM();

            var vm = new PackingMaterialVM
            {
                Id = packingMaterialData.Id,
                Type = ProductTypes.PackingMaterial,
                Name = packingMaterialData.Name,
                Make = packingMaterialData.Make,
                Pack = packingMaterialData.Pack,
                Rate = packingMaterialData.Rate,
                Gst = packingMaterialData.Gst ?? 0,
                Hsn = packingMaterialData.Hsn,
                Msl = packingMaterialData.Msl,
                CategoryId = packingMaterialData.CategoryId,
                CategoryName = packingMaterialData.Category.Name,
                UnitId = packingMaterialData.UnitId,
                Unit = packingMaterialData.Unit?.Name,
            };

            return vm;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(PackingMaterialVM vm)
        {
            var packingMaterial = new Product()
            {
                Id = vm.Id ?? Guid.Empty,
                Type = (int)ProductTypes.PackingMaterial,
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
                _db.Products.Add(packingMaterial);
            }
            else
            {
                _db.Products.Update(packingMaterial);
            }

            await _db.SaveChangesAsync();

            return new ServiceReturn<Guid>
            {
                Success = true,
                Message = vm.Id == null
                    ? "Packing material created successfully."
                    : "Packing material updated successfully.",
                Data = packingMaterial.Id
            };
        }
    }
}
