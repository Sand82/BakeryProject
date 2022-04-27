using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Contacts;
using Bakery.Service.Customers;

namespace Bakery.Service.Contacts
{
    public class ContactService : IContactService
    {        
        private readonly BakeryDbContext data;

        public ContactService( BakeryDbContext data)
        {           
            this.data = data;
        }

        public ContactFormModel FindModel(string userId)
        {
            ContactFormModel? model = null;

            Task.Run(() => 
            {
                model = this.data
                .Customers
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.Id)
                .Select(c => new ContactFormModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.PhoneNumber,
                    CustomerId = c.Id
                })
                .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return model;
        }

        public void CreateMail(ContactFormModel model)
        {
            MailInfo mail = new MailInfo 
            { 
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Massage = model.Massage,
                Email = model.Email,
                CustomerId = model.CustomerId
            };

            SaveMail(mail);
        }

        public void SaveMail(MailInfo model)
        {
            Task.Run(() => 
            { 
                this.data.MailInfos.Add(model);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();
        }
    }
}
