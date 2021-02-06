using System;

namespace Bookchin.Library.API.Interfaces
{
    public interface IDbModel : IVerifyable
    {
        Guid Id { get; set; }
        bool IsPersisted { get; }
    }
}