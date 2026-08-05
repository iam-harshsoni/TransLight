
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
    public class ProductService(TransLightContext db) : BaseService<Product>(db), IProductService
    {
        private TransLightContext _db = db;

        public async Task<PaginatedResponse<ProductVM>> GetProductAsync(ProductFilter filter)
        {
            var query = _db.Products.AsNoTracking().Include(x => x.Category).Include(x => x.Unit).Where(x => x.Type == (int)ProductTypes.Product);

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

            int totalProducts = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Type = ProductTypes.Product,
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

            return new PaginatedResponse<ProductVM>
            {
                Items = items,
                TotalItems = totalProducts,
                TotalPages = (int)Math.Ceiling((double)totalProducts / filter.PageSize),
                CurrentPage = filter.PageNumber
            };
        }

        public async Task<ProductVM> GetForEditAsync(Guid? id)
        {
            if (id == null)
                return new ProductVM();

            var productData = _db.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Unit)
                .Where(x => x.Id == id).FirstOrDefault();

            if (productData == null)
                return new ProductVM();

            var productVM = new ProductVM()
            {
                Id = productData.Id,
                Type = ProductTypes.Product,
                Name = productData.Name,
                Make = productData.Make,
                Pack = productData.Pack,
                Rate = productData.Rate,
                Gst = productData.Gst ?? 0,
                Hsn = productData.Hsn,
                Msl = productData.Msl,
                CategoryId = productData.CategoryId,
                CategoryName = productData.Category.Name,
                UnitId = productData.UnitId,
                Unit = productData.Unit?.Name,
                Active = (YesNo)productData.Active,

                RawMaterials = _db.ProductRawMaterials
                    .AsNoTracking()
                    .Where(x => x.ProductId == id && x.Type == (int)ProductTypes.RawMaterial)
                    .Select(x => new ProduceRawMaterialsVM
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        RawMaterialId = x.RawMaterialId,
                        RawMaterialName = x.RawMaterial.Name,
                        UnitId = x.UnitId,
                        UnitName = x.Unit != null ? x.Unit.Name : "",
                        Qty = x.Qty,
                        Type = (ProductTypes)x.Type,
                    })
                    .ToList(),

                PackingMaterials = _db.ProductRawMaterials
                    .AsNoTracking()
                    .Where(x => x.ProductId == id && x.Type == (int)ProductTypes.PackingMaterial)
                    .Select(x => new ProduceRawMaterialsVM
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        RawMaterialId = x.RawMaterialId,
                        RawMaterialName = x.RawMaterial.Name,
                        UnitId = x.UnitId,
                        UnitName = x.Unit != null ? x.Unit.Name : "",
                        Qty = x.Qty,
                        Type = (ProductTypes)x.Type,
                    })
                    .ToList(),
            };

            return productVM;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(ProductVM vm)
        {
            Product product;

            if (vm.Id == null)
            {
                product = new Product
                {
                    Id = Guid.Empty,
                    Type = (int)ProductTypes.Product
                };

                _db.Products.Add(product);
            }

            else
            {
                product = (await _db.Products
                    .Include(x => x.ProductRawMaterialProducts)
                    .Include(x => x.ProductRawMaterialRawMaterials)
                    .FirstOrDefaultAsync(x => x.Id == vm.Id))!;

                if (product == null)
                {
                    return new ServiceReturn<Guid>
                    {
                        Success = false,
                        Message = "Product not found."
                    };
                }
                _db.ProductRawMaterials.RemoveRange(product.ProductRawMaterialProducts);
            }

            product.Name = vm.Name;
            product.Make = vm.Make;
            product.Pack = vm.Pack;
            product.Rate = vm.Rate ?? 0;
            product.Gst = vm.Gst;
            product.CategoryId = vm.CategoryId;
            product.UnitId = vm.UnitId;
            product.Hsn = vm.Hsn;
            product.Msl = vm.Msl;
            product.Active = (int)vm.Active;

            #region Add/Update RawMaterials & ProduceMaterials
            // Update RawMaterials
            foreach (var item in vm.RawMaterials.Where(x => !x.IsSelected))
            {
                product.ProductRawMaterialProducts.Add(new ProductRawMaterial
                {
                    ProductId = product.Id,
                    RawMaterialId = item.RawMaterialId,
                    UnitId = item.UnitId,
                    Qty = item.Qty,
                    Type = (int)ProductTypes.RawMaterial
                });
            }

            // Update PackingMaterials
            foreach (var item in vm.PackingMaterials.Where(x => !x.IsSelected))
            {
                product.ProductRawMaterialProducts.Add(new ProductRawMaterial
                {
                    ProductId = product.Id,
                    RawMaterialId = item.RawMaterialId,
                    UnitId = item.UnitId,
                    Qty = item.Qty,
                    Type = (int)ProductTypes.PackingMaterial
                });
            }
            #endregion

            await _db.SaveChangesAsync();

            return new ServiceReturn<Guid>
            {
                Success = true,
                Message = vm.Id == null
                    ? "Product created successfully."
                    : "Product updated successfully.",
                Data = product.Id
            };
        }
    }
}

