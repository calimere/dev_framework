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
    public abstract class GenericRepository<W, T> : IGenericRepository<W, T> where W : DbContext where T : class
    {
        protected W _dbContext;
        protected SerilogManager _logger;

        public GenericRepository(W context, SerilogManager logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public virtual int GetTotal() { return _dbContext.Set<T>().Count(); }

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A DatabaseMessage indicating the result of the operation.</returns>
        public DatabaseMessage Add(T entity)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, entity);
            var retour = new DatabaseMessage(EnumDataBaseMessage.NoChanges);
            try { retour = add(entity); }
            catch (Exception ex)
            {
                _logger.Error(methodName, ex, entity, true);
            }
            _logger.Fin(methodName, retour.GetReturnValue<T>(), startTime);
            return retour;
        }
        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A DatabaseMessage indicating the result of the operation.</returns>
        public DatabaseMessage Add(T[] entity)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, entity);
            var retour = new DatabaseMessage(EnumDataBaseMessage.NoChanges);
            try { retour = add(entity); }
            catch (Exception ex)
            {
                _logger.Error<T>(methodName, ex, entity, true);
            }
            _logger.Fin(methodName, retour.GetReturnValue<T>(), startTime);
            return retour;
        }
        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A DatabaseMessage indicating the result of the operation.</returns>
        public DatabaseMessage Update(T[] entity)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, entity);
            var retour = new DatabaseMessage(EnumDataBaseMessage.NoChanges);
            try { retour = update(entity); }
            catch (Exception ex) { _logger.Error<T>(methodName, ex, entity, true); }
            _logger.Fin(methodName, retour.GetReturnValue<T>(), startTime);
            return retour;
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A DatabaseMessage indicating the result of the operation.</returns>
        public virtual DatabaseMessage Update(T entity)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, entity);
            var retour = new DatabaseMessage(EnumDataBaseMessage.NoChanges);
            try { retour = update(entity); }
            catch (Exception ex) { _logger.Error(methodName, ex, entity, true); }
            _logger.Fin(methodName, retour.GetReturnValue<T>(), startTime);
            return retour;
        }

        /// <summary>
        /// Retrieves an entity from the database by its ID.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The retrieved entity.</returns>
        public T GetItem(int id)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, id);
            T? entity = null;
            try { entity = _dbContext.Set<T>().Find(id); }
            catch (Exception ex) { _logger.ErrorById(methodName, ex, id); }
            _logger.Fin(methodName, entity, startTime);
            return entity;
        }
        /// <summary>
        /// Retrieves an entity from the database by its ID.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The retrieved entity.</returns>
        public T GetItem(string id)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, id);
            T? entity = null;
            try { entity = _dbContext.Set<T>().Find(id); }
            catch (Exception ex) { _logger.Error(methodName, ex, id); }
            _logger.Fin(methodName, entity, startTime);
            return entity;
        }

        /// <summary>
        /// Asynchronously retrieves an entity from the database by its ID.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the retrieved entity.</returns>
        public async Task<T> GetItemAsync(int id)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, id);
            T? entity = null;
            try { entity = await _dbContext.Set<T>().FindAsync(id); }
            catch (Exception ex) { _logger.ErrorById(methodName, ex, id); }
            _logger.Fin(methodName, entity, startTime);
            return entity;
        }
        public async Task<T> GetItemAsync(string id)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, id);
            T? entity = null;
            try { entity = await _dbContext.Set<T>().FindAsync(id); }
            catch (Exception ex) { _logger.Error(methodName, ex, id); }
            _logger.Fin(methodName, entity, startTime);
            return entity;
        }

        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the retrieved entities.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try { entities = _dbContext.Set<T>().ToArray(); }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }
        public virtual IEnumerable<T> GetAllAsc(int length)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try
            {
                entities = _dbContext.Set<T>().Take(length).AsEnumerable();
            }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName);
            IEnumerable<T> entities = null;
            try { entities = await _dbContext.Set<T>().ToArrayAsync(); }
            catch (Exception ex) { _logger.Error(methodName, ex); }
            _logger.Fin(methodName, entities, startTime);
            return entities;
        }

        /// <summary>
        /// Removes an entity from the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>A DatabaseMessage indicating the result of the operation.</returns>
        public DatabaseMessage Remove(T entity)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, entity);
            DatabaseMessage dbMessage = null;
            try
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
                var retour = _dbContext.SaveChanges();
                if (retour == -1) dbMessage = new DatabaseMessage(EnumDataBaseMessage.Error) { Count = 1 };
                else if (retour == 0) dbMessage = new DatabaseMessage(EnumDataBaseMessage.NoChanges) { Count = 1 };
                else dbMessage = new DatabaseMessage(EnumDataBaseMessage.Success) { Count = 1, ReturnValue = entity };
            }
            catch (Exception ex) { _logger.Error(methodName, ex, entity, true); }
            _logger.Fin(methodName, dbMessage.GetReturnValue<T>(), startTime);
            return dbMessage;
        }

        /// <summary>
        /// Removes multiple entities from the database.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="entities">The entities to be removed.</param>
        /// <returns>A DatabaseMessage indicating the result of the operation.</returns>
        public DatabaseMessage Remove(T[] entities)
        {
            var methodName = SerilogManager.GetCurrentMethod();
            var startTime = _logger.Debut(methodName, entities);
            DatabaseMessage dbMessage = null;
            try
            {
                foreach (var item in entities)
                    _dbContext.Entry(item).State = EntityState.Deleted;

                var retour = _dbContext.SaveChanges();
                if (retour == -1) dbMessage = new DatabaseMessage(EnumDataBaseMessage.Error) { Count = entities.Length, ReturnValue = entities };
                else if (retour == 0) dbMessage = new DatabaseMessage(EnumDataBaseMessage.NoChanges) { Count = entities.Length, ReturnValue = entities };
                else dbMessage = new DatabaseMessage(EnumDataBaseMessage.Success) { Count = entities.Length, ReturnValue = entities };
            }
            catch (Exception ex) { _logger.Error<T>(methodName, ex, entities, true); }
            _logger.Fin(methodName, dbMessage.GetReturnValue<T>(), startTime);
            return dbMessage;
        }

        /// <summary>
        /// Retrieves entities from the database based on a set of IDs and a key property. /!\ This method uses reflection.
        /// </summary>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="ids">The IDs of the entities to retrieve.</param>
        /// <param name="key">The key property to match the IDs against.</param>
        /// <returns>The retrieved entities.</returns>
        public IEnumerable<T> GetItems(int[] ids, string key)
        {
            return DbContextExtensions.FindEntities<T, int>(_dbContext, ids);
        }
        public IEnumerable<T> GetItems(string[] ids, string key)
        {
            return DbContextExtensions.FindEntities<T, string>(_dbContext, ids);
        }

        #region SQL

        protected string CreateWhereClauseFromCommand(List<KeyValuePair<string, object>> dic)
        {
            var lst = new List<string>();
            foreach (KeyValuePair<string, object> item in dic)
            {
                if (item.Value.GetType() == typeof(string))
                {
                    var value = (string)item.Value;
                    if (value.Contains("%"))
                        lst.Add(string.Format(" {1} like '{0}' ", value, item.Key));
                    else
                        lst.Add(string.Format(" {1} = '{0}' ", value, item.Key));
                }
                else if (item.Value.GetType() == typeof(DateTime))
                {
                    var value = (DateTime)item.Value;
                    var convertedDate = string.Format("{0}-{1}-{2} 00:00:00.0000000", value.Year, value.Month, value.Day);
                    lst.Add(string.Format(" {1} = '{0}' ", convertedDate, item.Key));
                }
                else if (item.Value.GetType() == typeof(int))
                {
                    var value = (int)item.Value;
                    lst.Add(string.Format(" {1} = '{0}' ", value, item.Key));
                }
                else if (item.Value.GetType() == typeof(object[]))
                {
                    var value = (object[])item.Value;
                    if (value.Any())
                        lst.Add(string.Format(" {1} in ({0}) ", string.Join(",", value), item.Key));
                }
                else if (item.Value.GetType() == typeof(SqlObjectOperator))
                {
                    var value = (SqlObjectOperator)item.Value;
                    if (value.Value.GetType() == typeof(int))
                    {
                        var v = (int)value.Value;
                        lst.Add(string.Format(" {2} {1} '{0}' ", v, value.Operator, item.Key));
                    }
                    else if (value.Value.GetType() == typeof(DateTime))
                    {
                        var date = (DateTime)value.Value;
                        var convertedDate = string.Format("{0}-{1}-{2} 00:00:00.0000000", date.Year, date.Month, date.Day);
                        lst.Add(string.Format(" {2} {1} '{0}' ", convertedDate, value.Operator, item.Key));
                    }
                }
            }
            return lst.Any() ? string.Format(" where {0}", string.Join("and", lst)) : string.Empty;
        }

        protected string CreateOffsetClauseFromCommand(string orderByField, int start, int length)
        {
            return string.Format(" order by {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", orderByField, start, length);
        }

        protected string CreateOffsetClauseFromCommand(DatatableOrder[] orders, int start, int length)
        {
            var orderByClause = CreateOrderByClause(orders);
            return string.Format(" {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", orderByClause, start, length);
        }

        protected string CreateOrderByClause(DatatableOrder[] orders)
        {
            var str = "order by ";

            var iterator = 0;
            foreach (var item in orders)
            {
                if (iterator > 0) str += ",";
                str += string.Format(" {0} {1} ", item.column + 1, item.dir);
                iterator++;
            }

            return str;
        }

        #endregion

        #region Private

        private DatabaseMessage add(T entity)
        {
            try
            {
                _dbContext.Add(entity);
                var retour = _dbContext.SaveChanges();

                if (retour > 0)
                    return new DatabaseMessage(EnumDataBaseMessage.Success) { Count = 1, ReturnValue = entity };

                return new DatabaseMessage(EnumDataBaseMessage.Error) { Count = 1, ReturnValue = retour };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private DatabaseMessage add(T[] entities)
        {
            try
            {
                _dbContext.AddRange(entities);
                var retour = _dbContext.SaveChanges();
                if (retour == -1)
                    return new DatabaseMessage(EnumDataBaseMessage.Error) { Count = entities.Length, ReturnValue = retour };

                return new DatabaseMessage(EnumDataBaseMessage.Success) { Count = retour, ReturnValue = entities };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private DatabaseMessage update(T entity)
        {
            try
            {
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

        private DatabaseMessage update(T[] entities)
        {
            try
            {
                foreach (var item in entities)
                    _dbContext.Entry(item).State = EntityState.Modified;
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

        private T GetPropValue<T>(object src, string propName) { return (T)src.GetType().GetProperty(propName).GetValue(src, null); }

        #endregion
    }

    public static class DbContextExtensions
    {
        public static T[] FindEntities<T, TKey>(this DbContext context, params TKey[] ids) where T : class
        {
            var dbSet = context.Set<T>();
            var keyProperty = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0];

            // Utilisation de LINQ pour rechercher les entités par leurs clés primaires
            return dbSet.Where(e => ids.Contains(EF.Property<TKey>(e, keyProperty.Name))).ToArray();
        }
    }
}
