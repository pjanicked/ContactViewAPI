namespace ContactViewAPI.App.Controllers
{
    using AutoMapper;
    using ContactViewAPI.App.Dtos.Contact;
    using ContactViewAPI.Data.Models;
    using ContactViewAPI.Service.Contact;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
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
            var contactToReturn = _mapper.Map<Contact, ContactToReturnDto>(contact);

            return Ok(contactToReturn);
        }
    }
}
