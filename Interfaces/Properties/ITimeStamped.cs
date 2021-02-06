using System;

namespace Bookchin.Library.API.Interfaces
{
    public interface ITimeStamped
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
    }
}