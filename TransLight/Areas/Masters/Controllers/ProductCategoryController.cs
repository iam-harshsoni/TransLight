using Microsoft.AspNetCore.Mvc;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Masters;
using TransLight.Services.Interfaces.Masters;
using TransLight.Utility.Enums;

namespace TransLight.Areas.Masters.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly ILogger<ProductCategoryController> _logger;
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService, ILogger<ProductCategoryController> logger)
        {
            _logger = logger;
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProductCategoriesData(string? name, int active = -1, int pageNumber = 1, int pageSize = 10)
        {
            var query = _productCategoryService.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (active > -1)
                query = query.Where(x => x.Active == active);

            int totalProductCategories = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductCategoryVM()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Active = ((YesNo)x.Active),
                }).ToList();

            return Json(new PaginatedResponse<ProductCategoryVM>
            {
                Items = items,
                TotalItems = totalProductCategories,
                TotalPages = (int)Math.Ceiling((double)totalProductCategories / pageSize),
                CurrentPage = pageNumber
            });
        }

        public IActionResult Upsert(Guid? id)
        {
            ProductCategoryVM productCategoryVM = new();
            if (id == null) return View(productCategoryVM);

            var productCatData = _productCategoryService.Get(x => x.Id == id);
            if (productCatData == null)
            {
                return NotFound();
            }

            productCategoryVM = new()
            {
                Id = productCatData.Id,
                Name = productCatData.Name ?? "",
                Active = (YesNo)productCatData.Active
            };

            return View(productCategoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Guid? id, ProductCategoryVM productCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategoryVM);
            }

            try
            {
                var productCategory = new ProductCategory()
                {
                    Id = productCategoryVM.Id ?? Guid.Empty,
                    Name = productCategoryVM.Name,
                    Active = (int)productCategoryVM.Active
                };

                if (productCategoryVM.Id == null)
                {
                    // create
                    _productCategoryService.Add(productCategory);
                    _productCategoryService.Save();

                    _logger.LogInformation($"New Category '{productCategoryVM.Name}' added successfully");
                    TempData["Success"] = "Category saved successfully.";
                }
                else
                {
                    _productCategoryService.Update(productCategory);
                    _productCategoryService.Save();

                    _logger.LogInformation($"Category '{productCategoryVM.Name}' updated successfully");
                    TempData["Success"] = "Category updated successfully.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Error saving the Category in the database {ex.Message}");
                TempData["Error"] = "Error saving the Category.";
            }

            return View(productCategoryVM);
        }
    }
}
