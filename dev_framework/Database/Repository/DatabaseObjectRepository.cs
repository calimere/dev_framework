using dev_framework.Form.Model.Datatable;
using dev_framework.Manager;
using dev_framework.Message.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Database.Repository
{
    public abstract class DatabaseObjectRepository<W, T> : GenericRepository<W, T>, IDatabaseObjectRepository<W, T> where W : DbContext where T : DatabaseObject
    {
        public DatabaseObjectRepository(W context, SerilogManager logger) : base(context, logger) { }

        public virtual IEnumerable<T> GetAllDesc(int length)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try
            {
                entities = _dbContext.Set<T>().OrderByDescending(m => m.created).Take(length).AsEnumerable();
            }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }
    }
}
