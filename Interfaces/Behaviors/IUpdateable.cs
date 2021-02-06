namespace Bookchin.Library.API.Interfaces
{
    public interface IUpdateable<T, K>
    {
        T UpdateFromVm(K vm);
    }
}