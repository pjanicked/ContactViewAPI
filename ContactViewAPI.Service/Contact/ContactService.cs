namespace ContactViewAPI.Service.Contact
{
    using ContactViewAPI.Data;
    using ContactViewAPI.Data.Models;
    using ContactViewAPI.Service.Contact.Pagination;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ContactService : IContactService
    {
        private readonly ContactDbContext _dbContext;

        public ContactService(ContactDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contact> CreateAsync(Contact contact, int? userId)
        {
            contact.CreatedAt = DateTime.UtcNow;
            contact.UserId = (int)userId;

            _dbContext.Contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> DeleteAsync(int? contactId, int? userId)
        {
            var dbContact = await ValidateAsync(contactId, userId);

            _dbContext.Contacts.Remove(dbContact);
            await _dbContext.SaveChangesAsync();
            return true;          
        }

        public async Task<PagedList<Contact>> GetAllContactsByUserId(UserParams userParams)
        {
            var contacts = _dbContext.Contacts
                .Include(i => i.Notes)
                .OrderByDescending(o => o.CreatedAt)
                .Where(c => c.UserId == userParams.UserId).AsQueryable();

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                contacts = userParams.OrderBy switch
                {
                    "updated" => contacts.OrderByDescending(u => u.UpdatedAt),
                    _ => contacts.OrderByDescending(u => u.CreatedAt),
                };
            }

            return await PagedList<Contact>.CreateAsync(contacts, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Contact> GetContactById(int? contactId, int? userId)
        {
            var dbContact = await ValidateAsync(contactId, userId);

            return dbContact;
        }

        public async Task<Contact> UpdateAsync(Contact contact, int? userId)
        {
            var dbContact = await ValidateAsync(contact.Id, userId);

            dbContact = Map(dbContact, contact);

            _dbContext.Contacts.Update(dbContact);
            await _dbContext.SaveChangesAsync();

            return dbContact;
        }

        private Contact Map(Contact dbContact, Contact contact)
        {
            dbContact.Name = contact.Name;
            dbContact.Company = contact.Company;
            dbContact.Email = contact.Email;
            dbContact.Phone = contact.Phone;
            dbContact.Address = contact.Address;
            dbContact.Description = contact.Description; 
            dbContact.CompanyUrl = contact.CompanyUrl;
            dbContact.UpdatedAt = DateTime.UtcNow;

            foreach (var existingChild in dbContact.Notes.ToList())
            {
                if (!contact.Notes.Any(n => n.Id == existingChild.Id))
                    _dbContext.Notes.Remove(existingChild);
            }

            foreach (var childUpdateModel in contact.Notes.ToList())
            {
                var dbNote = dbContact.Notes.SingleOrDefault(n => n.Id == childUpdateModel.Id && n.Id != 0);
                if (dbNote != null)
                {
                    dbNote.NoteText = childUpdateModel.NoteText;
                    dbNote.UpdatedAt = DateTime.UtcNow;

                    _dbContext.Notes.Update(dbNote);
                }
                else
                {
                    childUpdateModel.CreatedAt = DateTime.UtcNow;
                    childUpdateModel.ContactId = dbContact.Id;

                    _dbContext.Notes.Add(childUpdateModel);
                }
            }

            return dbContact;
        }

        private async Task<Contact> ValidateAsync(int? contactId, int? userId)
        {
            if (contactId is null || contactId <= 0)
                throw new Exception("contactId cannot be null");

            var dbContact = await _dbContext.Contacts.Include(n => n.Notes).FirstOrDefaultAsync(c => c.Id == contactId);

            if (dbContact is null)
                throw new Exception("this contact does not exist");

            if (dbContact.UserId != userId)
                throw new Exception("This user is not the owner of contact Id");

            return dbContact;
        }
    }
}
