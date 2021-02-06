namespace Bookchin.Library.API.Controllers.ViewModels
{
    public class IndividualViewModel
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        
        public AddressViewModel Address { get; set; }
    }
}