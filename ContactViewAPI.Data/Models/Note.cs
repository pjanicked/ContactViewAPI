namespace ContactViewAPI.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

#nullable enable
    public class Note : IEntityTypeConfiguration<Note>
    {
        public int Id { get; set; }
        public string? NoteText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ContactId { get; set; }
        public Contact? Contact { get; set; }

        public void Configure(EntityTypeBuilder<Note> builder)
        {
        }
    }
}
