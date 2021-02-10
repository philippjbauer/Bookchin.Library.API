namespace Bookchin.Library.API.Interfaces
{
    public interface IUserAccount : IDbModel, IJsonable
    {
        string DisplayName { get; }
        string ShortName { get; }
    }
}