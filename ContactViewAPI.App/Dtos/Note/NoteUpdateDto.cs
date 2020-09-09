namespace ContactViewAPI.App.Dtos.Note
{
#nullable enable
    public class NoteUpdateDto
    {
        public int Id { get; set; }
        public string? NoteText { get; set; }
        public int ContactId { get; set; }
    }
}
