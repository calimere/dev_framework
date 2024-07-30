using dev_framework.Database.Interface;
using dev_framework.Form.Model.Datatable;
using dev_framework.Manager;
using dev_framework.Message.Model;
using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dev_framework.Database.Repository
{
    public abstract class BusinessGenericRepository<W, T> : DatabaseObjectRepository<W, T>, IBusinessGenericRepository<W, T> where W : DbContext where T : BusinessObject
    {
        public BusinessGenericRepository(W context, SerilogManager logger) : base(context, logger)
        {
        }

        public override IEnumerable<T> GetAll()
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try
            {
                entities = _dbContext.Set<T>().Where(m => !m.is_deleted).AsEnumerable();
            }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }
        public override IEnumerable<T> GetAllAsc(int length)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try
            {
                entities = _dbContext.Set<T>().Where(m => !m.is_deleted).Take(length).AsEnumerable();
            }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }
        public override IEnumerable<T> GetAllDesc(int length)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try
            {
                entities = _dbContext.Set<T>().Where(m => !m.is_deleted).OrderByDescending(m => m.created).Take(length).AsEnumerable();
            }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }
        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try
            {
                entities = await _dbContext.Set<T>().Where(m => !m.is_deleted).ToArrayAsync();
            }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }

        public override int GetTotal() { return _dbContext.Set<T>().Where(m => !m.is_deleted).Count(); }
        public DatabaseMessage DeleteItem(int id)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, id);
            DatabaseMessage retour = null;

            var entity = GetItem(id);
            try { retour = entity != null ? Delete(entity) : new DatabaseMessage(EnumDataBaseMessage.NoChanges); }
            catch (Exception ex) { _logger.Error(methodName, ex, entity); }
            _logger.Fin(methodName, retour.GetReturnValue<T>(), startTime);
            return retour;
        }
        public DatabaseMessage DeleteItems(int[] ids, string key)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, ids);
            DatabaseMessage retour = null;

            var items = GetItems(ids, key);
            try { retour = items != null ? Delete(items.ToArray()) : new DatabaseMessage(EnumDataBaseMessage.NoChanges); }
            catch (Exception ex) { _logger.Error(methodName, ex, items); }
            _logger.Fin(methodName, retour.GetReturnValue<T[]>(), startTime);
            return retour;
        }

        #region Private

        private DatabaseMessage Delete(T entity)
        {
            try
            {
                entity.is_deleted = true;
                _dbContext.Entry(entity).State = EntityState.Modified;
                var retour = _dbContext.SaveChanges();
                if (retour == -1) return new DatabaseMessage(EnumDataBaseMessage.Error) { Count = 1, ReturnValue = entity };
                else if (retour == 0) return new DatabaseMessage(EnumDataBaseMessage.NoChanges) { Count = 1, ReturnValue = entity };
                else return new DatabaseMessage(EnumDataBaseMessage.Success) { Count = 1, ReturnValue = entity };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private DatabaseMessage Delete(T[] entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    item.is_deleted = true;
                    _dbContext.Entry(item).State = EntityState.Modified;
                }

                var retour = _dbContext.SaveChanges();
                if (retour == -1) return new DatabaseMessage(EnumDataBaseMessage.Error) { Count = entities.Length, ReturnValue = entities };
                else if (retour == 0) return new DatabaseMessage(EnumDataBaseMessage.NoChanges) { Count = entities.Length, ReturnValue = entities };
                else return new DatabaseMessage(EnumDataBaseMessage.Success) { Count = entities.Length, ReturnValue = entities };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}
