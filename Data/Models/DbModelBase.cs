using System;
using System.Text.Json.Serialization;
using Bookchin.Library.API.Interfaces;

namespace Bookchin.Library.API.Data.Models
{
    public abstract class DbModelBase : IDbModel, ITimeStamped
    {
        // Identifier Properties
        public Guid Id { get; set; }

        // System Properties
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        // Dynamic Properties
        [JsonIgnore]
        public bool IsPersisted => this.Id != Guid.Empty;
        public abstract bool IsValid { get; }

        public DbModelBase() { }

        public abstract bool Verify();
    }
}