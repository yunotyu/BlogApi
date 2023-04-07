using Blog.Api.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Services
{
    /// <summary>
    /// 通用数据库操作接口类
    /// </summary>
    /// <typeparam name="TDbContext">对应的数据库dbcontext</typeparam>
    /// <typeparam name="TEntity">对应的实体类</typeparam>
    public class BaseServices<TDbContext, TEntity> : IBaseServices<TEntity> where TDbContext : DbContext where TEntity : class, new()
    {
        protected readonly TDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseServices(TDbContext dbContext, DbSet<TEntity> dbSet)
        {
            this._dbContext = dbContext;
            this._dbSet = dbSet;
        }

        public async Task<int> Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Add(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<long> Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate==null?await _dbSet.CountAsync():await _dbSet.CountAsync(predicate);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            //软删除，将isDel字段置为1
            string delProName = "IsDel";
            entity.GetType().GetProperty(delProName).SetValue(entity,1);
            _dbSet.Update(entity);
            int count = await _dbContext.SaveChangesAsync();
            return count == 1 ? true : false;
        }

        public async Task<bool> Delete(List<TEntity> entities)
        {

            for (int i = 0; i < entities.Count; i++)
            {
                //软删除，将isDel字段置为1
                string delProName = "IsDel";
                entities[i].GetType().GetProperty(delProName).SetValue(entities[i], 1);
            }
            _dbContext.UpdateRange(entities);
            int count = await _dbContext.SaveChangesAsync();
            return count == entities.Count ? true : false;
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ?await _dbSet.AnyAsync() :await _dbSet.AnyAsync(predicate);
        }

        public async Task<TEntity> QueryById(object objId)
        {
            return await _dbSet.FindAsync(objId);
        }


        public IQueryable<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order = null, Expression<Func<TEntity, TEntity>> selection = null, bool isTracking = true)
        {
            IQueryable<TEntity> query = isTracking ? _dbSet : _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (order != null)
            {
                query = order(query);
            }
            if (selection != null)
            {
                query = query.Select(selection);
            }
            return query;
        }

        public async Task<bool> Update(TEntity entity)
        {
            _dbSet.Update(entity);
            int count= await _dbContext.SaveChangesAsync();
            return count==1?true:false;
        }

        public async Task<bool> Update(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            int count = await _dbContext.SaveChangesAsync();
            return count == entities.Count ? true : false;
        }
    }
}
