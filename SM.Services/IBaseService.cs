using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Services
{
    public interface IBaseService<T> where T : class
    {
        void Add(T obj, bool flush = true);
        void Update(T obj, bool flush = true);
        IQueryable<T> GetAll();
        void Delete(T entity, bool flush = true);

        Task AddAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(T obj);

        [Obsolete("Detach is obsolete. Use DetachByClone instead")]
        T Detach(T entity, string[] includes = null);
        [Obsolete("Detach is obsolete. Use DetachByClone instead")]
        IList<T> Detach(IList<T> entity, string[] includes = null);

        T DetachByClone(T entity, string[] includes = null);
        IList<T> DetachByClone(IList<T> entity, string[] includes = null);
        IList<object> DetachByClone(IList<object> entity, string[] includes = null);

        void Flush();
        Task FlushAsync();
        void DisableProxy();
        void DisableTrackChange();
        void DisableLazyLoading();
    }
}
