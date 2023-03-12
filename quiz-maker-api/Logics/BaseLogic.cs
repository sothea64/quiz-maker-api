using Microsoft.EntityFrameworkCore;
using quiz_maker_api.Helpers;
using quiz_maker_models.Helpers;
using quiz_maker_models.Models;
using quiz_maker_models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace quiz_maker_api.Logics
{
    public class BaseLogic<T> where T : BaseModel
    {
        public QuizMakerDbContext _db;
        public readonly ICurrentScope _current;

        public BaseLogic(QuizMakerDbContext db, ICurrentScope current)
        {
            _db = db;
            _current = current;
        }

        public virtual async Task<T> FindAsync(int id, bool include = false)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<bool?> CanRemoveAsync(int id)
        {
            var entity = await FindAsync(id);
            return await CanRemoveAsync(entity);
        }

        public virtual async Task<bool?> CanRemoveAsync(T entity)
        {
            return await Task.FromResult(true);
        }

        public virtual async Task<bool?> IsExistsAsync(T entity)
        {
            return await Task.FromResult(false);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (await IsExistsAsync(entity)??true)
            {
                throw new DuplicateException(nameof(entity));
            }
            _db.Entry(entity).State = EntityState.Detached;
            entity.CreatedBy = _current?.UserName ?? "-";
            entity.CreatedDate = DateTime.Now;
            entity.Active = true;
            //entity.Active = true;
            _db.Entry(entity).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (await IsExistsAsync(entity)??true)
            {
                throw new DuplicateException(nameof(entity));
            }
            _db.Entry(entity).State = EntityState.Detached;
            entity.ModifiedDate = DateTime.Now;
            await UpdateUnsavedAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> RemoveAsync(int id)
        {
            var entity = await FindAsync(id, true);
            if (!await CanRemoveAsync(entity)??false)
            {
                throw new InUsedException(nameof(entity));
            }
            return await RemoveAsync(entity);
        }

        public virtual async Task<T> RemoveAsync(T entity)
        {
            entity.Active = false;
            entity.ModifiedDate = DateTime.Now;
            await UpdateUnsavedAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Update the entity without db.SaveChanges.
        /// Useful for update detail object inside other batch transaction.
        /// </summary>
        /// <param name="dyn"></param>
        /// <returns></returns>
        public async Task<T> UpdateUnsavedAsync(T entity)
        {
            _db.Entry(entity).State = EntityState.Detached;
            entity.ModifiedDate = DateTime.Now;

            var cur = await _db.Set<T>().FindAsync(entity.Id);
            _db.Entry(cur).CurrentValues.SetValues(entity);

            return cur;
        }

        public virtual IQueryable<T> Search(ISearchModel search)
        {
            return _db.Set<T>().Where(x => x.Active);
        }

        public async virtual Task<List<T>> SearchAsync(ISearchModel search)
        {
            return await Search(search).ToListAsync();
        }

        /// <summary>
        /// Execute async fn inside transaction manager.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="timeout">timeout in second - default 10 minutes</param>
        /// <returns></returns> 
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, int timeout = 10 * 60)
        {
            _db.Database.SetCommandTimeout(TimeSpan.FromSeconds(timeout));
            using (var tran = new TransactionScope(
              scopeOption: TransactionScopeOption.Required,
              transactionOptions: new TransactionOptions()
              {
                  IsolationLevel = IsolationLevel.ReadCommitted,
                  Timeout = TimeSpan.FromSeconds(timeout)
              },
              asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled
              ))
            {
                if (_db.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                {
                    _db.Database.GetDbConnection().Open();
                }

                T t = await action?.Invoke();
                tran.Complete();
                return t;
            }
        }

        public async Task<List<TEntity>> UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseModel
        {
            var distinctIds = entities.Select(x => x.Id).Distinct().ToList();
            var dbEntities = await _db.Set<TEntity>().Where(x => distinctIds.Contains(x.Id)).ToListAsync();

            foreach(var entity in dbEntities)
            {
                entity.ModifiedDate = DateTime.Now;
                var sourceEntity = entities.FirstOrDefault(x => x.Id == entity.Id);
                
                if (sourceEntity != null)
                {
                    _db.Entry(sourceEntity).State = EntityState.Detached;
                    _db.Entry(entity).CurrentValues.SetValues(sourceEntity);
                    _db.Entry(entity).State = EntityState.Modified;
                }
            }

            await _db.SaveChangesAsync();
            return dbEntities;
        }

        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseModel
        {
            foreach (var entity in entities)
            {
                _db.Entry(entity).State = EntityState.Detached;
                entity.Active = true;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = _current?.UserName ?? "-";
                _db.Entry(entity).State = EntityState.Added;
            }

            await _db.SaveChangesAsync();
        }
    }
}
