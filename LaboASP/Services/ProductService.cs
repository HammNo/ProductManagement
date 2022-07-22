using Demo.ASP.Services;
using ProductManagement.DAL.Entities;
using ProductManagement.ASP.Exceptions;
using ProductManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.ASP.Services
{
    public class ProductService
    {
        private readonly ProdManagementContext _dc;
        private readonly MailService _mailService;

        public ProductService(ProdManagementContext dc, MailService mailService)
        {
            _dc = dc;
            _mailService = mailService;
        }

        public IEnumerable<Product> GetProducts(bool getAll)
        {
            if (!getAll) return _dc.Products.OrderBy(p => p.Reference).Where(p => p.Delete == false);
            else return _dc.Products.OrderBy(p => p.Reference);
        }

        public Product GetByReference(string reference)
        {
            Product? product = _dc.Products.SingleOrDefault(p => p.Reference == reference);
            if(product == null) throw new ModelException(nameof(product), "Produit introuvable");
            else return product;
        }

        public Product GetById(int id, bool active)
        {
            Product? product = _dc.Products.Include(p => p.Categories).SingleOrDefault(p => p.Id == id);
            if (product != null) 
            {   
                if(product.Delete == !active) return product;
            }
            throw new ModelException(nameof(product), "Produit introuvable");
        }
        public IEnumerable<Product> SearchByName(string searchedName)
        {
            return _dc.Products.Include(p => p.Categories).Where(p => p.Name.Contains(searchedName) && p.Delete == false);
        }


        public void CreateProduct(Product product)
        {
            if (product is null)
            {
                throw new ModelException(nameof(product), "Erreur de création du produit");
            }
            else
            {
                product.CreationDate = DateTime.Now;
                product.UpdateDate = product.CreationDate;
                product.Delete = false;
                IEnumerable<Product> sameName_prod = GetProducts(true)
                    .Where(p => p.Reference.Substring(0,4) == product.Name.Replace(" ", "").Substring(0,4));
                string reference_end = (sameName_prod.Count() + 1).ToString().PadLeft(4, '0');
                product.Reference = product.Name.Replace(" ", "").Substring(0, 4) + reference_end;
                _dc.Products.Add(product);
                _dc.SaveChanges();
            }
        }

        public void Delete(Product product)
        {
            if(product != null && product.Delete == false)
            {
                decimal value = product.Stock * product.Price;
                if(value > 1000)
                {
                    throw new ModelException("H1000", "La valeur totale est supérieure à 1000 euros");
                }
                product.Delete = true;
                product.UpdateDate= DateTime.Now;
                _dc.SaveChanges();
                if(value > 100)
                {
                    string subject = $"Suppression de l'article {product.Reference}";
                    string email = "nabilhammoud@hotmail.com";
                    string content = $"Nom produit : {product.Name}\n" +
                                     $"Description : {product.Description}\n" +
                                     $"Date de création : {product.CreationDate}\n" +
                                     $"Prix : {product.Price}";
                    _mailService.Send(subject, content, email);
                }
            }
            else
            {
                throw new ModelException(nameof(product), "Suppression impossible");
            }
        }

        public void UpdateProduct(Product newProduct)
        {
            if (newProduct != null)
            {
                try
                {
                    Product oldProduct = GetById(newProduct.Id, true);
                    if(newProduct.Name != oldProduct.Name) oldProduct.Name = newProduct.Name;
                    if (newProduct.Description != oldProduct.Description) oldProduct.Description = newProduct.Description;
                    if (newProduct.Price != oldProduct.Price) oldProduct.Price = newProduct.Price;
                    if(newProduct.Stock != oldProduct.Stock)
                    {
                        int stockOffset =  newProduct.Stock - oldProduct.Stock;
                        decimal value = (oldProduct.Price * stockOffset) * -1;
                        if(newProduct.Stock >= 0)
                        {
                            if (value > 100)
                            {
                                string subject = $"Modification de l'article {oldProduct.Reference}";
                                string email = "nabilhammoud@hotmail.com";
                                string content = $"Nom produit : {oldProduct.Name}\n" +
                                                 $"Description : {oldProduct.Description}\n" +
                                                 $"Date de création : {oldProduct.CreationDate}\n" +
                                                 $"Prix : {oldProduct.Price}\n" +
                                                 $"Valeur des pièces débitées : {value} euros";
                                _mailService.Send(subject, content, email);
                            }
                            oldProduct.Stock = newProduct.Stock;
                        }
                        else
                        {
                            stockOffset = oldProduct.Stock - newProduct.Stock;
                            value = oldProduct.Price * stockOffset;
                            oldProduct.Stock = 0;
                            string subject = $"Modification de l'article {oldProduct.Reference}";
                            string email = "nabilhammoud@hotmail.com";
                            string content = $"Tentative de suppression de pièces en plus grand nombre que le stock.\n" +
                                             $"Les pièces disponibles ont été débitées, le stock est à présent de 0.\n" +
                                             $"Nom produit : {oldProduct.Name}\n" +
                                             $"Description : {oldProduct.Description}\n" +
                                             $"Date de création : {oldProduct.CreationDate}\n" +
                                             $"Prix : {oldProduct.Price}\n" +
                                             $"Valeur des pièces débitées : {value} euros";
                            _mailService.Send(subject, content, email);
                        }
                    }
                    oldProduct.UpdateDate = DateTime.Now;
                    oldProduct.Categories.Clear();
                    oldProduct.Categories = newProduct.Categories;
                    _dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ModelException("Modification produit", ex.Message);
                }
            }
            else
            {
                throw new ModelException(nameof(newProduct), "Modification impossible");
            }
        }
    }
}
