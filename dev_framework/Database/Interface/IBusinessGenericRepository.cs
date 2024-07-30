using dev_framework.Database.Repository;
using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace dev_framework.Database.Interface
{
    public interface IBusinessGenericRepository<W,T> : IDatabaseObjectRepository<W,T> where W: DbContext where T : BusinessObject
    {
        DatabaseMessage DeleteItem(int id);
        DatabaseMessage DeleteItems(int[] ids, string key);
        int GetTotal();
    }
}
