namespace Bookchin.Library.API.Interfaces
{
    public interface IVerifyable
    {
        bool IsValid { get; }
        bool Verify();
    }
}