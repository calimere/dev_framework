using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_framework.Database
{
    public interface IGenericRepository<W, T> where W : DbContext where T : class
    {
        DatabaseMessage Add(string methodName, T entity);
        DatabaseMessage Update(string methodName, T entity);
        T GetItem(string methodName, int id);
        T GetItem(string methodName, string id);
        Task<T> GetItemAsync(string methodName, int id);
        Task<T> GetItemAsync(string methodName, string id);

        Task<IEnumerable<T>> GetAll(string methodName);
        DatabaseMessage Remove(string methodName, T entity);
        DatabaseMessage Remove(string methodName, T[] entities);
        IEnumerable<T> GetItems(string methodName, int[] ids, string key);
        IEnumerable<T> GetItems(string methodName, string[] ids, string key);
    }
}
