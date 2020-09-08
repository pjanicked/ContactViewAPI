namespace ContactViewAPI.App.Dtos.Note
{

#nullable enable
    public class NoteToReturnDto
    {
        public int Id { get; set; }
        public string? NoteText { get; set; }
        public int ContactId { get; set; }
    }
}
