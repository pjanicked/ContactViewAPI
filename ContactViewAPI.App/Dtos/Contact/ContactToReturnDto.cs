namespace ContactViewAPI.App.Dtos.Contact
{
    using ContactViewAPI.App.Dtos.Note;
    using System;
    using System.Collections.Generic;

#nullable enable
    public class ContactToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? CompanyUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<NoteToReturnDto>? Notes { get; set; }
    }
}
