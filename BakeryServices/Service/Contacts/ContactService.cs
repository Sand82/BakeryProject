using BakeryData;
using BakeryData.Models;
using BakeryServices.Models.Contacts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BakeryServices.Service.Contacts
{
    public class ContactService : IContactService
    {
        private readonly BakeryDbContext data;
        private readonly IConfiguration configuration;

        public ContactService(BakeryDbContext data, IConfiguration configuration)
        {
            this.data = data;
            this.configuration = configuration;
        }

        public async Task<ContactFormModel> FindModel(string userId)
        {
            var model = await this.data
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
            .FirstOrDefaultAsync();

            return model;
        }

        public async Task CreateMail(ContactFormModel model)
        {
            var mail = new MailInfo
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Massage = model.Massage,
                Email = model.Email,
                CustomerId = model.CustomerId,
                Subject = model.Subject,

            };

            await SaveMail(mail);
        }

        public async Task SaveMail(MailInfo model)
        {
            await this.data.MailInfos.AddAsync(model);

            await this.data.SaveChangesAsync();
        }

        public async Task AddMailToSendGrid(string senderMailAddres, string senderName, string currentSubject, string message)
        {
            var apiKey = configuration["ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(senderMailAddres, senderName);
            var subject = currentSubject;
            var to = new EmailAddress(configuration["BakeryMailAddres"], "Vqra Hristova");
            var plainTextContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, plainTextContent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

        }
    }
}
