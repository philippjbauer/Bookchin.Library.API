using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Exceptions;
using Bookchin.Library.API.Interfaces;

namespace Bookchin.Library.API.Data.Models
{
    public abstract class UserAccount : DbModelBase, IUserAccount
    {
        // Identifier Properties
        [JsonIgnore]
        public string Discriminator { get; private set; }

        // Model Properties
        [Required]
        public virtual Address Address { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Dynamic Properties
        public abstract string DisplayName { get; }
        public abstract string ShortName { get; }
        public string UserType => this.Discriminator;
        public bool IsActive => this.ApplicationUser.IsActive;

        public override bool IsValid
        {
            get
            {
                try
                {
                    return this.Address.Verify() && Verify();
                }
                catch (ModelValidationException)
                {
                    return false;
                }
            }
        }

        public UserAccount() : base() { }

        public UserAccount(AddressViewModel vm)
        {
            this.Address = new Address(vm);
        }

        public string ToJson() => JsonSerializer.Serialize(this);
    }
}
