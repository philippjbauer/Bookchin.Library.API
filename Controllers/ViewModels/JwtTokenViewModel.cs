using System;

namespace Bookchin.Library.API.Controllers.ViewModels
{
    public class JwtTokenViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}