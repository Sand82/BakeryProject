using BakeryData.Models;
using BakeryServices.Models.Contacts;

namespace BakeryServices.Service.Contacts
{
    public interface IContactService 
    {
        Task<ContactFormModel> FindModel(string userId);

        Task CreateMail(ContactFormModel model);

        Task SaveMail(MailInfo model);

        Task AddMailToSendGrid(string senderMailAddres, string senderName, string subject, string message);
    }
}
