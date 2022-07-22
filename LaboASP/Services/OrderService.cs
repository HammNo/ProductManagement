using Demo.ASP.Services;
using Microsoft.EntityFrameworkCore;
using ProductManagement.ASP.Exceptions;
using ProductManagement.DAL;
using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Services
{
    public class OrderService
    {
        private readonly ProdManagementContext _dc;
        private readonly MailService _mailService;
        private readonly ProductService _productService;

        public OrderService(ProdManagementContext dc, MailService mailService, ProductService productService)
        {
            _dc = dc;
            _mailService = mailService;
            _productService = productService;
        }
        public IEnumerable<Order> GetOrders()
        {
            return _dc.Orders.Include(o => o.Client).Include(o => o.OrderLines);
        }
        public Order GetById(int id)
        {
            Order? order = _dc.Orders
                           .Include(o => o.Client)
                           .Include(o => o.OrderLines)
                           .ThenInclude(ol => ol.Product)
                           .SingleOrDefault(o => o.Id == id);
            if (order != null) return order;
            throw new ModelException(nameof(order), "Commande introuvable");
        }

        public void CreateOrder(Order order, IEnumerable<OrderLine>? orderLines)
        {
            if (order is null)
            {
                throw new ModelException(nameof(order), "Erreur de création de la commande");
            }
            else
            {
                order.CreationDate = DateTime.Now;
                order.UpdateDate = order.CreationDate;
                string reference_date = order.CreationDate.Year.ToString().Substring(order.CreationDate.Year.ToString().Length - 2)
                                        + order.CreationDate.Month.ToString().PadLeft(2, '0')
                                        + order.CreationDate.Day.ToString();
                IEnumerable<Order> sameDate_ord = GetOrders()
                    .Where(o => o.Reference.Substring(0, 6) == reference_date).OrderByDescending(o => int.Parse(o.Reference.Substring(6)));
                string reference_end;
                if (sameDate_ord.Count() > 0)
                {
                    reference_end = (int.Parse(sameDate_ord.First().Reference.Substring(6)) + 1).ToString().PadLeft(4, '0');
                }
                else reference_end = "0001";
                order.Reference = reference_date + reference_end;
                _dc.Orders.Add(order);
                _dc.SaveChanges();
                if(orderLines != null)
                {
                    foreach (OrderLine orderL in orderLines)
                    {
                        orderL.OrderId = order.Id;
                        CreateOL(orderL);
                    }
                }
            }
        }

        public void CreateOL(OrderLine orderLine)
        {
            if (orderLine is null)
            {
                throw new ModelException(nameof(orderLine), "Erreur de création de la ligne de commande");
            }
            try
            {
                Product product = _productService.GetById(orderLine.ProductId, true);
                if(orderLine.Quantity > product.Stock)
                {
                    throw (new ModelException(nameof(orderLine), "Impossible d'ajouter le produit car le stock est insuffisant"));
                }
                else
                {
                    _dc.OrderLines.Add(orderLine);
                    _dc.SaveChanges();
                }
            }
            catch(ModelException ex)
            {
                throw (ex);
            }
        }

        public void Delete(Order order)
        {
            if (order != null)
            {
                _dc.Remove(order);
                _dc.SaveChanges();
            }
            else
            {
                throw new ModelException(nameof(order), "Suppression impossible");
            }
        }
    }
}
