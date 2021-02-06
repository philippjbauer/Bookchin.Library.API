using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bookchin.Library.API.Interfaces
{
    public interface IModelRepository<T>
    {
        List<T> List(Expression<Func<T, bool>> predicate = null);
        T Read(Guid id);
        T Add(T record);
        List<T> AddRange(List<T> records);
        T Update(T record);
        void Delete(Guid id);
        void Delete(T record);
    }
}