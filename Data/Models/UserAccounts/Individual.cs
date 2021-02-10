using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Helpers;
using Bookchin.Library.API.Interfaces;

namespace Bookchin.Library.API.Data.Models
{
    public class Individual : UserAccount, IUpdateable<Individual, IndividualViewModel>
    {
        // Model Properties
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Suffix { get; set; }

        // Dynamic Properties
        public override string DisplayName
        {
            get
            {
                List<string> possiblyEmptyNameParts = new List<string>
                {
                    this.Title,
                    this.FirstName,
                    this.MiddleName,
                    this.LastName,
                    this.Suffix
                };

                List<string> nameParts = possiblyEmptyNameParts
                    .FindAll(p => string.IsNullOrEmpty(p?.Trim()) == false);

                return string.Join(" ", nameParts);
            }
        }

        public override string ShortName => $"{this.FirstName.Substring(0, 1)}{this.LastName.Substring(0, 1)}".ToUpper();

        // Internal Properties
        protected readonly List<string> _requiredProperties = new List<string>
        {
            "FirstName", "LastName"
        };

        public Individual()
            : base() { }

        public Individual(IndividualViewModel vm)
            : base(vm.Address)
        {
            UpdateFromVm(vm);
        }

        public Individual UpdateFromVm(IndividualViewModel vm)
        {
            if (vm != null)
            {
                this.Title = vm.Title;
                this.FirstName = vm.FirstName;
                this.MiddleName = vm.MiddleName;
                this.LastName = vm.LastName;
                this.Suffix = vm.Suffix;

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