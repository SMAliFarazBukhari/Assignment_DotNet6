using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Assignment_DotNet6.Entities
{
    public class Doctor : IdentityUser
    {
        [Key]
        public Guid D_ID { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Password { get; set; }
    }
}
