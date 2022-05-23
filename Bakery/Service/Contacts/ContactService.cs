using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Contacts;
using Bakery.Service.Customers;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Bakery.Service.Contacts
{
    public class ContactService : IContactService
    {        
        private readonly BakeryDbContext data;
        private readonly IConfiguration configuration;

        public ContactService( BakeryDbContext data, IConfiguration configuration)
        {           
            this.data = data;
            this.configuration = configuration;
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
                CustomerId = model.CustomerId,
                Subject = model.Subject,             
               
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

        public void AddMailToSendGrid(string senderMailAddres, string senderName, string currentSubject, string message)
        {
            string apiKey = null;
            SendGridClient client = null;
            EmailAddress from = null;
            EmailAddress to = null;

            Task.Run(() =>
            {             
                apiKey = configuration["ApiKey"];
                client = new SendGridClient(apiKey);
                from = new EmailAddress(senderMailAddres, senderName);
                var subject = currentSubject;
                to = new EmailAddress(configuration["BakeryMailAddres"], "Vqra Hristova");
                var plainTextContent = message;              
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, plainTextContent);
                var response = client.SendEmailAsync(msg).ConfigureAwait(false);

            }).GetAwaiter().GetResult();
        }
    }
}
