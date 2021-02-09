namespace ContactViewAPI.App.Dtos.Contact
{
#nullable enable
    public class ContactToListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Company { get; set; }
        public string? Phone { get; set; }
        public int? NumberOfNotes { get; set; }
    }
}
