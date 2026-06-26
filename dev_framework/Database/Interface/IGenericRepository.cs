using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_framework.Database
{
    public interface IGenericRepository<W, T> where W : DbContext where T : class
    {
        DatabaseMessage Add(T entity);
        DatabaseMessage Add(T[] entity);

        DatabaseMessage Update(T entity);
        T GetItem(int id);
        T GetItem(string id);
        T GetItem(Guid id);
        Task<T> GetItemAsync(int id);
        Task<T> GetItemAsync(string id);
        Task<T> GetItemAsync(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllAsc(int length);
        Task<IEnumerable<T>> GetAllAsync();
        DatabaseMessage Remove(T entity);
        DatabaseMessage Remove(T[] entities);
        IEnumerable<T> GetItems(int[] ids, string key);
        IEnumerable<T> GetItems(string[] ids, string key);
        IEnumerable<T> GetItems(Guid[] ids, string key);
        int GetTotal();
    }
}
