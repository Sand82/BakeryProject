using Bakery.Data.Models;
using Bakery.Models.Contacts;

namespace Bakery.Service.Contacts
{
    public interface IContactService 
    {
        ContactFormModel FindModel(string userId);

        void CreateMail(ContactFormModel model);

        void SaveMail(MailInfo model);

        void AddMailToSendGrid(string senderMailAddres, string senderName, string subject, string message);
    }
}
