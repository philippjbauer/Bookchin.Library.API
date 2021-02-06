using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Helpers;
using Bookchin.Library.API.Interfaces;

namespace Bookchin.Library.API.Data.Models
{
    public class Organization : User, IUpdateable<Organization, OrganizationViewModel>
    {
        // Model Properties
        [Required]
        public string Name { get; set; }

        // Dynamic Properties
        public override string DisplayName => this.Name;
        public override string ShortName => this.Name.Substring(0, 1).ToUpper();

        // Internal Properties
        protected readonly List<string> _requiredProperties = new List<string>
        {
            "Name"
        };

        public Organization()
            : base() { }

        public Organization(OrganizationViewModel vm)
            : base(vm.Address)
        {
            UpdateFromVm(vm);
        }

        public Organization UpdateFromVm(OrganizationViewModel vm)
        {
            if (vm != null)
            {
                this.Name = vm.Name;

                // Don't accidentally remove an address.
                if (vm.Address != null)
                {
                    this.Address = this.Address
                        .UpdateFromVm(vm.Address);
                }
            }

            Verify();

            return this;
        }

        public override bool Verify() =>
            ModelValidationHelpers.HasRequiredClassProperties(this, _requiredProperties);
    }
}