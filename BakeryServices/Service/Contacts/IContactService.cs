using Bakery.Data.Models;
using Bakery.Models.Contacts;

namespace Bakery.Service.Contacts
{
    public interface IContactService 
    {
        Task<ContactFormModel> FindModel(string userId);

        Task CreateMail(ContactFormModel model);

        Task SaveMail(MailInfo model);

        Task AddMailToSendGrid(string senderMailAddres, string senderName, string subject, string message);
    }
}
