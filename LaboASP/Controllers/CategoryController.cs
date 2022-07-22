using Microsoft.AspNetCore.Mvc;
using ProductManagement.ASP.Exceptions;
using ProductManagement.ASP.Models.CategoryVM;
using ProductManagement.ASP.Services;
using ProductManagement.ASP.Utils;
using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {

            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryIndexViewModel> categories = _categoryService.GetCategories(false).Select(c => new CategoryIndexViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description?? ""
            });
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = new Category
                    {
                        Name = model.Name,
                        Description = model.Description
                    };
                    _categoryService.CreateCategory(category);
                    TempData.Success("Création réssie");
                    return RedirectToAction("Index");
                }
                catch (ModelException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                }
            }
            TempData.Error("Vérifiez vos données");
            return View(model);
        }

        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                Category category = _categoryService.GetById(id, true);
                _categoryService.Delete(category);
                TempData.Success("Suppression réussie");
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
                Category category = _categoryService.GetById(id, true);
                CategoryEditViewModel model = new CategoryEditViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                };
                return View(model);
            }
            catch(ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = new Category
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                    };
                    _categoryService.UpdateCategory(category);
                    TempData.Success("Modification réussie");
                    return RedirectToAction("Index");
                }
                catch (ModelException ex)
                {
                    ModelState.AddModelError(ex.Field, ex.Message);
                }
            }
            TempData.Error("Vérifiez vos données");
            return View(model);
        }
    }
}
