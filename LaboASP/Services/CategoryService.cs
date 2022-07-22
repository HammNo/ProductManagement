using Microsoft.EntityFrameworkCore;
using ProductManagement.ASP.Exceptions;
using ProductManagement.DAL;
using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Services
{
    public class CategoryService
    {
        private readonly ProdManagementContext _dc;

        public CategoryService(ProdManagementContext dc)
        {
            _dc = dc;
        }
        public IEnumerable<Category> GetCategories(bool getAll)
        {
            if (!getAll) return _dc.Categories.Where(c => c.Delete == false);
            else return _dc.Categories;
        }

        public Category GetById(int id, bool active)
        {
            Category? category = _dc.Categories.Include(c => c.Products).SingleOrDefault(c => c.Id == id);
            if (category != null)
            {
                if(category.Delete == !active)
                {
                    return category;
                }
            }
            throw new ModelException(nameof(category), "Catégorie introuvable");
        }
        public IEnumerable<Category> GetFromIds(List<int> ids)
        {
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    yield return GetById(id, true);
                }
            }
            else yield return null;
        }
        public void CreateCategory(Category category)
        {
            if (category is null)
            {
                throw new ModelException(nameof(category), "Erreur de création de la catégorie");
            }
            else
            {
                category.Delete = false;
                IEnumerable<Category> sameName_cat = GetCategories(true)
                    .Where(c => c.Name == category.Name);
               if(sameName_cat.Count() > 0) throw new ModelException(nameof(category), "Une catégorie à ce nom existe déjà!");
                _dc.Categories.Add(category);
                _dc.SaveChanges();
            }
        }
        public void Delete(Category category)
        {
            if (category != null)
            {
                category.Products = null;
                category.Delete = true;
                _dc.SaveChanges();
            }
            else
            {
                throw new ModelException(nameof(category), "Suppression impossible");
            }
        }

        public void UpdateCategory(Category newCategory)
        {
            if (newCategory != null)
            {
                try
                {
                    Category oldCategory = GetById(newCategory.Id, true);
                    if (newCategory.Name != oldCategory.Name) oldCategory.Name = newCategory.Name;
                    if (newCategory.Description != oldCategory.Description) oldCategory.Description = newCategory.Description;
                    _dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ModelException("Modification catégorie", ex.Message);
                }
            }
            else
            {
                throw new ModelException(nameof(newCategory), "Modification impossible");
            }
        }
    }
}
