namespace ContactViewAPI.Service.Identity
{
    using ContactViewAPI.Data.Models;

    public interface IUserService
    {
        string GenerateJwt(User user);
    }
}
