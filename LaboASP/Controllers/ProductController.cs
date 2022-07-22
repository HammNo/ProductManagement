using Demo.ASP.Services;
using ProductManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.ASP.Exceptions;
using ProductManagement.ASP.Services;
using System.Text.Json;
using ProductManagement.ASP.Utils;
using ProductManagement.ASP.Models.ProductVM;

namespace ProductManagement.ASP.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        public ProductController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View(GetProducts("", 0,  5));
        }

        [HttpGet]
        public IActionResult Index(string search, int page, bool hasChSea)
        {
            return View(GetProducts(search, page, 5));
        }

        private ProductPageIndexViewModel GetProducts(string searchedName, int pageIndex, int maxResults)
        {
            ProductPageIndexViewModel products_page = new ProductPageIndexViewModel();
            IEnumerable<Product> products;
            if (string.IsNullOrEmpty(searchedName) && pageIndex == 0)
            {
                products = _productService.GetProducts(false);
                products_page.NbrPages = (products.Count() / maxResults) + ((products.Count() % maxResults == 0) ? 0 : 1);
                products_page.CurrentPage = 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(searchedName))
                {
                    products = _productService.SearchByName(searchedName);
                    products_page.SearchedName = searchedName;
                }
                else
                {
                    products = _productService.GetProducts(false);
                }
                products_page.NbrPages = (products.Count() / maxResults) + ((products.Count() % maxResults == 0) ? 0 : 1);
                products_page.CurrentPage = pageIndex;
            }
            products_page.PageProducts = products
                .Skip((products_page.CurrentPage - 1) * maxResults)
                .Take(maxResults)
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Reference = p.Reference,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                });
            return products_page;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductCreateViewModel model = new ProductCreateViewModel
            {
                AllCategories = _categoryService.GetCategories(false).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    decimal price = this.PriceConversion(model.Price);
                    ICollection<Category>? categories = null;
                    if (model.SelectedCategories != null)
                    {
                        categories = _categoryService.GetFromIds(model.SelectedCategories).ToList();
                    }
                    Product product = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = price,
                        Stock = model.Stock,
                        Categories = categories
                    };
                    _productService.CreateProduct(product);
                    TempData.Success("Création réussie");
                    return RedirectToAction("Index");
                }
                catch (ModelException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                }
            }
            model.AllCategories = _categoryService.GetCategories(false).ToList();
            TempData.Error("Vérifiez vos données");
            return View(model);
        }

        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                Product product = _productService.GetById(id, true);
                _productService.Delete(product);
                TempData.Success("Suppression réussie");
            }
            catch (ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details([FromRoute] int id)
        {
            try {
                Product product = _productService.GetById(id, true);
                ProductDetailsViewModel model = new ProductDetailsViewModel
                {
                    Id = product.Id,
                    Reference = product.Reference,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreationDate = product.CreationDate,
                    UpdateDate = product.UpdateDate,
                    Categories = product.Categories.Select(c => c.Name).ToList()
                };
                return Json(model);
            }
            catch (ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
                Product product = _productService.GetById(id, true);
                ProductEditViewModel model = new ProductEditViewModel
                {
                    Id = product.Id,
                    Reference = product.Reference,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price.ToString(),
                    Stock = product.Stock,
                    SelectedCategories = product.Categories.Select(c => c.Id).ToList()
                };
                model.AllCategories = _categoryService.GetCategories(false).ToList();
                return View(model);
            }
            catch(ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    decimal price = this.PriceConversion(model.Price);
                    ICollection<Category>? categories = null;
                    if (model.SelectedCategories != null)
                    {
                        categories = _categoryService.GetFromIds(model.SelectedCategories).ToList();
                    }
                    Product product = new Product
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Price = price,
                        Stock = model.Stock + (model.StockOffset ?? 0),
                        Categories = categories
                    };
                    _productService.UpdateProduct(product);
                    TempData.Success("Modification réussie");
                    return RedirectToAction("Index");
                }
                catch (ModelException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                }
            }
            model.AllCategories = _categoryService.GetCategories(false).ToList();
            TempData.Error("Vérifiez vos données"); 
            return View(model);
        }

        [HttpGet]
        public ActionResult GetFromName(string name)
        {
            try
            {
                IEnumerable<ProductIndexViewModel> results = _productService.SearchByName(name)
                   .Select(c => new ProductIndexViewModel
                   {
                       Id = c.Id,
                       Name=c.Name,
                       Price = c.Price,
                       Reference = c.Reference
                   });
                return Json(results);
            }
            catch (ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                Product product = _productService.GetById(id, true);
                ProductDetailsViewModel result = new ProductDetailsViewModel
                   {
                       Id = product.Id,
                       Name = product.Name,
                       Price = product.Price,
                       Reference = product.Reference,
                       Description = product.Description,
                       Stock = product.Stock,
                   };
                return Json(result);
            }
            catch (ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
