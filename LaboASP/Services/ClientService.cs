using ProductManagement.ASP.Exceptions;
using ProductManagement.DAL;
using ProductManagement.DAL.Entities;

namespace ProductManagement.ASP.Services
{
    public class ClientService
    {
        private readonly ProdManagementContext _dc;

        public ClientService(ProdManagementContext dc)
        {
            _dc = dc;
        }
        public IEnumerable<Client> GetClients()
        {
            return _dc.Clients;
        }
        public Client GetById(int id)
        {
            Client? client = _dc.Clients.SingleOrDefault(c => c.Id == id);
            if (client == null) throw new ModelException(nameof(client), "Client introuvable");
            else return client;
        }

        public void CreateClient(Client client)
        {
            if (client is null)
            {
                throw new ModelException(nameof(client), "Erreur de création du client");
            }
            else
            {
                client.CreationDate = DateTime.Now;
                client.UpdateDate = client.CreationDate;
                IEnumerable<Client> sameName_clients = GetClients()
                    .Where(c => c.Reference.Substring(0, 4) == (client.LastName.Replace(" ", "").Substring(0, 2) + client.FirstName.Replace(" ", "").Substring(0, 2)));
                string reference_end = (sameName_clients.Count() + 1).ToString().PadLeft(4, '0');
                client.Reference = (client.LastName.Replace(" ", "").Substring(0, 2) + client.FirstName.Replace(" ", "").Substring(0, 2)) + reference_end;
                _dc.Clients.Add(client);
                _dc.SaveChanges();
            }
        }
        public void Delete(Client client)
        {
            if (client != null)
            {
                _dc.Remove(client);
                _dc.SaveChanges();
            }
            else
            {
                throw new ModelException(nameof(client), "Suppression impossible");
            }
        }

        public void UpdateClient(Client newClient)
        {
            if (newClient != null)
            {
                try
                {
                    Client oldClient = GetById(newClient.Id);
                    if (newClient.FirstName != oldClient.FirstName) oldClient.FirstName = newClient.FirstName;   
                    if (newClient.LastName != oldClient.LastName) oldClient.LastName = newClient.LastName;   
                    if (newClient.Mail != oldClient.Mail) oldClient.Mail = newClient.Mail;   
                    if (newClient.Gender != oldClient.Gender) oldClient.Gender =  newClient.Gender;
                    oldClient.UpdateDate = DateTime.Now;
                    _dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ModelException("Modification client", ex.Message);
                }
            }
            else
            {
                throw new ModelException(nameof(newClient), "Modification impossible");
            }
        }

        public IEnumerable<Client> SearchByName(string name)
        {
            return _dc.Clients.Where(c => c.LastName.Contains(name));
        }
    }
}
