using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductManagement.ASP.Exceptions;
using ProductManagement.ASP.Models.OrderVM;
using ProductManagement.ASP.Models.ProductVM;
using ProductManagement.ASP.Services;
using ProductManagement.ASP.Utils;
using ProductManagement.DAL.Entities;
using Toolbox.Enums;

namespace ProductManagement.ASP.Controllers
{
    public class OrderController : Controller
    {

        public OrderService _orderService;
        public ClientService _clientService;

        public OrderController(OrderService orderService, ClientService clientService)
        {
            _orderService = orderService;
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            IEnumerable<Order> trs = _orderService.GetOrders();
            IEnumerable<OrderIndexViewModel> orders = _orderService.GetOrders().Select(o => new OrderIndexViewModel
            {
                Id = o.Id,
                Reference = o.Reference,
                Status = DescriptionAttribute.GetValue(o.Status),
                ClientName = $"{o.Client.FirstName} {o.Client.LastName}",
            });
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            OrderCreateViewModel model = new OrderCreateViewModel();
            return View(new OrderCreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(OrderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<OrderLine>? orderLines = null;
                    if(model.OrderLines != null)
                    {
                        orderLines = new List<OrderLine>();
                        foreach (string line in model.OrderLines)
                        {
                            if(!string.IsNullOrEmpty(line))orderLines.Add(JsonConvert.DeserializeObject<OrderLine>(line));
                        }
                    }
                    _clientService.GetById(model.ClientId);
                    Order order = new Order
                    {
                        ClientId = model.ClientId
                    };
                    _orderService.CreateOrder(order, orderLines);

                    //List<OrderLine> orderLines = new List<OrderLine>();
                    //foreach(OrderLineCreateViewModel line in model.OrderLines)
                    //{
                    //    if (line != null)
                    //    {
                    //        OrderLine newOrderLine = new OrderLine
                    //        {
                    //            Quantity = line.Quantity,
                    //            ProductId = line.ProductId,
                    //        };
                    //    }
                    //}

                    TempData.Success("Création réussie");
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
                Order order = _orderService.GetById(id);
                _orderService.Delete(order);
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
                Order order = _orderService.GetById(id);
                OrderEditViewModel model = new OrderEditViewModel
                {
                    Id = order.Id,
                    Reference = order.Reference,
                    Status = DescriptionAttribute.GetValue(order.Status),
                    ClientName = order.Client.FirstName + " " + order.Client.LastName,
                    ExistingOrderLines = order.OrderLines.Select(ol => new OrderLineEditViewModel
                    {
                        Id = ol.Id,
                        Quantity = ol.Quantity,
                        Price = ol.Quantity,
                        Product = new ProductIndexViewModel
                        {
                            Id = ol.Product.Id,
                            Reference = ol.Product.Reference,
                            Name = ol.Product.Name,
                            Price = ol.Product.Price,
                            Stock = ol.Product.Stock,
                        },
                    }),
                };
                return View(model);
            }
            catch (ModelException ex)
            {
                TempData.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(ProductEditViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        decimal price = this.PriceConversion(model.Price);
            //        ICollection<Category>? categories = null;
            //        if (model.SelectedCategories != null)
            //        {
            //            categories = _categoryService.GetFromIds(model.SelectedCategories).ToList();
            //        }
            //        Product product = new Product
            //        {
            //            Id = model.Id,
            //            Name = model.Name,
            //            Description = model.Description,
            //            Price = price,
            //            Stock = model.Stock + (model.StockOffset ?? 0),
            //            Categories = categories
            //        };
            //        _productService.UpdateProduct(product);
            //        TempData.Success("Modification réussie");
            //        return RedirectToAction("Index");
            //    }
            //    catch (ModelException ex)
            //    {
            //        ModelState.AddModelError(ex.Field, ex.Message);
            //    }
            //}
            //model.AllCategories = _categoryService.GetCategories(false).ToList();
            //TempData.Error("Vérifiez vos données");
            return View(model);
        }
    }
}
