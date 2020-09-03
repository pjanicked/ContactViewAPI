namespace ContactViewAPI.App.Controllers
{
    using AutoMapper;
    using ContactViewAPI.App.Dtos.Auth;
    using ContactViewAPI.Data.Models;
    using ContactViewAPI.Service.Email;
    using ContactViewAPI.Service.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        public AuthController(IMapper mapper, UserManager<User> userManager, IEmailSender emailSender,
            IUserService userService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            var user = _mapper.Map<UserForRegisterDto, User>(userForRegister);

            var userCreated = await _userManager.CreateAsync(user, userForRegister.Password);
            
            if(userCreated.Succeeded)
            {
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = "Hi, Welcome to ContactView! Please click on this link to complete registration " + 
                    Url.Action(nameof(ConfirmEmail), "Auth", 
                    new { emailToken, email = user.Email }, Request.Scheme);

                var message = new Message(new string[] { user.Email }, "Confirmation Email Link", confirmationLink, null);
                await _emailSender.SendEmailAsync(message);

                return Ok();
            }

            return BadRequest(userCreated.Errors);
        }

        /// <summary>
        /// Clicking on the verification link in the users email inbox redirects to this action
        /// After succesfully registering and verifying, assign token to user and redirect to Contacts page
        /// </summary>
        /// <param name="emailToken"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("confirmemail")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ConfirmEmail(string emailToken, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, emailToken);
            if(result.Succeeded)
            {
                return Ok(_userService.GenerateJwt(user));
            }

            return BadRequest(result.Errors);
        }       

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserForLoginDto userForLogin)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == userForLogin.Email);
            if (user is null)
            {
                return NotFound("User not found");
            }

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if(!emailConfirmed)
            {
                return BadRequest("Email is not confirmed!");
            }    

            var userSigninResult = await _userManager.CheckPasswordAsync(user, userForLogin.Password);

            if (userSigninResult)
            {
                return Ok(_userService.GenerateJwt(user));
            }

            return BadRequest("Email or password incorrect.");
        }
        
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordDto userForgotPassword)
        {
            var user = await  _userManager.FindByEmailAsync(userForgotPassword.Email);
            if(user is null)
            {
                return NotFound("User not found");
            }

            var emailToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var forgotPasswordLink = "Hi, Please click on this link to reset your password " +
                Url.Action(nameof(GenerateResetPassword), "Auth",
                new { emailToken, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset Password Link", forgotPasswordLink, null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpGet("resetpassword")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GenerateResetPassword(string emailToken, string email)
        {
            return Ok(new
            {
                emailToken,
                email
            });
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);

            if (user is null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
    }
}
