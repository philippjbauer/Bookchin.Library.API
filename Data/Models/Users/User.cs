using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Exceptions;
using Bookchin.Library.API.Interfaces;

namespace Bookchin.Library.API.Data.Models
{
    public abstract class User : DbModelBase, IUser
    {
        // Identifier Properties
        [JsonIgnore]
        public string Discriminator { get; private set; }

        // Model Properties
        [Required]
        public bool IsActive { get; set; } = false;

        [Required]
        public virtual Address Address { get; set; }

        // Dynamic Properties
        public abstract string DisplayName { get; }
        public abstract string ShortName { get; }
        public string UserType => this.Discriminator;

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

        public User() : base() { }

        public User(AddressViewModel vm)
        {
            this.Address = new Address(vm);
        }

        public string ToJson() => JsonSerializer.Serialize(this);
    }
}
