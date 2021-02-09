namespace ContactViewAPI.App.Controllers
{
    using AutoMapper;
    using ContactViewAPI.App.Dtos.Contact;
    using ContactViewAPI.App.Helpers.Common;
    using ContactViewAPI.Data.Models;
    using ContactViewAPI.Service.Contact;
    using ContactViewAPI.Service.Contact.Pagination;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Route("api/contact")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContactService _contactService;
        private readonly UserManager<User> _userManager;

        public ContactController(IMapper mapper, IContactService contactService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _contactService = contactService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDto createContactDto)
        {
            var contact = _mapper.Map<ContactCreateDto, Contact>(createContactDto);

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            contact = await _contactService.CreateAsync(contact, user.Id);
            var contactToReturn = _mapper.Map<Contact, ContactToReturnDto>(contact);

            return CreatedAtRoute("GetContact", new { contact.Id }, contactToReturn);
        }

        [HttpGet("{Id}", Name = "GetContact")]
        public async Task<IActionResult> GetContact(int? Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            var contact = await _contactService.GetContactById(Id, user.Id);

            return Ok(_mapper.Map<Contact, ContactToReturnDto>(contact));
        }

        [HttpGet("test")]
        public IActionResult TestAction()
        {
            return Ok(new string("just a test endpoint"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContactsByUser([FromQuery] UserParams userParams)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            userParams.UserId = currentUserId;

            var contacts = await _contactService.GetAllContactsByUserId(userParams);
            var contactsToReturn = _mapper.Map<IEnumerable<ContactToListDto>>(contacts);
            Response.AddPaginationHeader(contacts.CurrentPage, contacts.PageSize, contacts.TotalPages, contacts.TotalCount);

            return Ok(contactsToReturn);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateContact(int? Id, ContactUpdateDto contactUpdateDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            var contact = _mapper.Map<ContactUpdateDto, Contact>(contactUpdateDto);
            contact.Id = (int)Id;
            await _contactService.UpdateAsync(contact, user.Id);

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteContact(int? Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            await _contactService.DeleteAsync(Id, user.Id);
            return NoContent();
        }
    }
}
