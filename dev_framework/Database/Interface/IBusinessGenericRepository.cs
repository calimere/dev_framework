using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace dev_framework.Database.Interface
{
    public interface IBusinessGenericRepository<W,T> : IGenericRepository<W,T> where W: DbContext where T : BusinessObject
    {
        IEnumerable<T> GetAll(bool withDeleted = false);
        DatabaseMessage DeleteItem(int id);
        DatabaseMessage DeleteItems(int[] ids, string key);
    }
}
