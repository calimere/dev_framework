using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_framework.Database
{
    public interface IDatabaseObjectRepository<W, T> : IGenericRepository<W, T> where W : DbContext where T : DatabaseObject
    {
        IEnumerable<T> GetAllDesc(int length);
    }
}
