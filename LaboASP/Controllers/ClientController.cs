using Microsoft.AspNetCore.Mvc;
using ProductManagement.ASP.Exceptions;
using ProductManagement.ASP.Models.ClientVM;
using ProductManagement.ASP.Services;
using ProductManagement.ASP.Utils;
using ProductManagement.DAL.Entities;
using Toolbox.Enums;

namespace ProductManagement.ASP.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            IEnumerable<ClientIndexViewModel> clients = _clientService.GetClients().Select(c => new ClientIndexViewModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Reference = c.Reference
            });
            return View(clients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ClientCreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(ClientCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Client client = new Client
                    {
                        FirstName = model.FirstName,
                        LastName= model.LastName,
                        Mail = model.Mail,
                        Gender = (GenderType) model.SelectedGender
                    };
                    _clientService.CreateClient(client);
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
            Client client = _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }
            try
            {
                _clientService.Delete(client);
                TempData.Success("Client supprimé");
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
            try
            {
                Client client = _clientService.GetById(id);
                Object model = new
                {
                    Id = id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Reference = client.Reference,
                    Mail = client.Mail,
                    CreationDate = client.CreationDate.ToShortDateString(),
                    Gender = DescriptionAttribute.GetValue(client.Gender),
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
                Client client = _clientService.GetById(id);
                ClientEditViewModel model = new ClientEditViewModel
                {
                    Id = client.Id,
                    Reference = client.Reference,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Mail = client.Mail,
                    SelectedGender = (int) client.Gender
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
        public IActionResult Edit(ClientEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Client client = new Client
                    {
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Mail = model.Mail,
                        Gender = (GenderType)model.SelectedGender
                    };
                    _clientService.UpdateClient(client);
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

        [HttpGet]
        public ActionResult GetFromName(string name)
        {
            try
            {
                IEnumerable<ClientIndexViewModel> results = _clientService.SearchByName(name)
                   .Select(c => new ClientIndexViewModel
                   {
                       Id = c.Id,
                       FirstName = c.FirstName,
                       LastName = c.LastName,
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
    }
}
