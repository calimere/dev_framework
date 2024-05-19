using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace dev_framework.Database.Interface
{
    public interface IBusinessGenericRepository<W,T> where W: DbContext where T : BusinessObject
    {
        IEnumerable<T> GetAll(string methodName, bool withDeleted = false);
        DatabaseMessage DeleteItem(string methodName, int id);
        DatabaseMessage DeleteItems(string methodName, int[] ids, string key);
    }
}
