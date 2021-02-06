using System;

namespace Bookchin.Library.API.Interfaces
{
    public interface IUser : IDbModel, IJsonable
    {
        string DisplayName { get; }
        string ShortName { get; }
    }
}