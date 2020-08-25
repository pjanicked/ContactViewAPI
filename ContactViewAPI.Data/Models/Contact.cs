namespace ContactViewAPI.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;

#nullable enable
    public class Contact : IEntityTypeConfiguration<Contact>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? CompanyUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Note>? Notes { get; set; }

        public void Configure(EntityTypeBuilder<Contact> builder)
        {            
        }
    }
}
