using Blog.Api.IServices;
using Blog.Api.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    /// <typeparam name="TEntity">对应的实体类</typeparam>
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> _dbSet;
        public BlogsqlContext DbContext { get; set; }
        public IDbContextTransaction DbContextTransaction { get; set; }

        //这里的DbContext在具体Services对象注入时会在构造函数注入
        public BaseServices(BlogsqlContext DbContext)
        {
            this.DbContext = DbContext;
            this._dbSet = DbContext.Set<TEntity>();
        }


        public async Task<int> Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Add(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return await DbContext.SaveChangesAsync();
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
            int count = await DbContext.SaveChangesAsync();
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
            DbContext.UpdateRange(entities);
            int count = await DbContext.SaveChangesAsync();
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

        public IQueryable<TEntity> QueryPage(int pageIndex, int pageCount)
        {
            return _dbSet.Skip((pageIndex - 1)*pageCount).Take(pageCount);
        }

        public async Task<bool> Update(TEntity entity)
        {
            _dbSet.Update(entity);
            int count= await DbContext.SaveChangesAsync();
            return count==1?true:false;
        }

        public async Task<bool> Update(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            int count = await DbContext.SaveChangesAsync();
            return count == entities.Count ? true : false;
        }

        public IQueryable<TEntity> QueryAll()
        {
            return _dbSet;
        }

        public void BeginTransaction()
        {
            DbContextTransaction = DbContext.Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            DbContextTransaction.Rollback();
        }

        public void Commit()
        {
            DbContextTransaction.Commit();
        }

       
    }
}
