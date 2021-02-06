using System.Text.Json;
using System.Collections.Generic;
using Bookchin.Library.API.Helpers;
using Bookchin.Library.API.Interfaces;
using System.ComponentModel.DataAnnotations;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Exceptions;

namespace Bookchin.Library.API.Data.Models
{
    public class Address : DbModelBase, IAddress, IUpdateable<Address, AddressViewModel>
    {
        // Model Properties
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }

        // Dynamic Properties
        public override bool IsValid
        {
            get
            {
                try
                {
                    return Verify();
                }
                catch (ModelValidationException)
                {
                    return false;
                }
            }
        }

        // Internal Properties
        protected readonly List<string> _requiredProperties = new List<string>
        {
            "Street", "City", "PostalCode", "State", "Country"
        };

        public Address() { }

        public Address(AddressViewModel vm)
        {
            UpdateFromVm(vm);
        }

        public Address UpdateFromVm(AddressViewModel vm)
        {
            if (vm != null)
            {
                this.Street = vm.Street;
                this.City = vm.City;
                this.State = vm.State;
                this.PostalCode = vm.PostalCode;
                this.Country = vm.Country;
            }

            Verify();

            return this;
        }

        public override bool Verify() =>
            ModelValidationHelpers.HasRequiredClassProperties(this, _requiredProperties);

        public string ToJson() =>
            JsonSerializer.Serialize<Address>(this, new JsonSerializerOptions { MaxDepth = 2 });
    }
}
