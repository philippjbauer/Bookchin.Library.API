using System.Threading.Tasks;

namespace Bookchin.Library.API.Interfaces
{
    public interface ILendable
    {
        Task<bool> LendAsync();
        Task<bool> ReturnAsync();
    }
}