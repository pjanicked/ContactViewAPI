namespace ContactViewAPI.Service.Contact
{
    using ContactViewAPI.Service.Contact.Pagination;
    using System.Threading.Tasks;

    public interface IContactService
    {
        Task<Data.Models.Contact> CreateAsync(Data.Models.Contact contact, int? userId);

        Task<Data.Models.Contact> UpdateAsync(Data.Models.Contact contact, int? userId);

        Task<bool> DeleteAsync(int? contactId, int? userId);

        Task<Data.Models.Contact> GetContactById(int? contactId, int? userId);

        Task<PagedList<Data.Models.Contact>> GetAllContactsByUserId(UserParams userParams);
    }
}
