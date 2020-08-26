namespace ContactViewAPI.Service.Identity
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Secret { get; set; }

        public int ExpirationInDays { get; set; }
    }
}
