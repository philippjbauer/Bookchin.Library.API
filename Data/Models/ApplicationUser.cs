using System;
using System.ComponentModel.DataAnnotations;
using Bookchin.Library.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bookchin.Library.API.Data.Models
{
    public class ApplicationUser : IdentityUser, ITimeStamped
    {
        // Model Properties
        [Required]
        public bool IsActive { get; set; } = false;
        
        public Guid UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        // System Properties
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}