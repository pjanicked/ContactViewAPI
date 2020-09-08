namespace ContactViewAPI.App.Dtos.Contact
{
#nullable enable
    public class ContactCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? CompanyUrl { get; set; }
    }
}
    