using Blog.Api.IServices;
using Blog.Api.Model;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;
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


        public async Task<bool> Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return await DbContext.SaveChangesAsync()==1;
        }

        public async Task Add(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddBulk(List<TEntity> entities)
        {
            await DbContext.BulkInsertAsync(entities);
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

        public async Task Delete(List<TEntity> entities)
        {
            //软删除，只更新isDel字段
            await DbContext.BulkUpdateAsync(entities);
        }

        public async Task DeleteData(List<TEntity> entities)
        {
             _dbSet.RemoveRange(entities);
           await DbContext.SaveChangesAsync();
        }

        public async Task BulkDeleteData(List<TEntity> entities)
        {
            await DbContext.BulkDeleteAsync(entities);
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ?await _dbSet.AnyAsync() :await _dbSet.AnyAsync(predicate);
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

        public IQueryable<TEntity> QueryPage(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> express=null)
        {
            if (express == null)
            {
                return _dbSet.Skip((pageIndex - 1) * pageCount).Take(pageCount);
            }
            return _dbSet.Where(express).Skip((pageIndex - 1)*pageCount).Take(pageCount);
        }

        public async Task<bool> Update(TEntity entity)
        {
            _dbSet.Update(entity);
            int count= await DbContext.SaveChangesAsync();
            return count==1?true:false;
        }

        public async Task Update(List<TEntity> entities)
        {
            //_dbSet.UpdateRange(entities);
            //await _dbSet.Where(p=> ids.Contains((int)p.GetType().GetProperty("Id").GetValue(p))).ExecuteUpdateAsync();
            //int count = await DbContext.SaveChangesAsync();
            //return count == entities.Count ? true : false;
            await DbContext.BulkUpdateAsync<TEntity>(entities);
            //_dbSet.UpdateRange(entities);
        }

        public async Task<bool> Update(TEntity entity, Expression<Func<TEntity, object>>[] pros)
        {
            _dbSet.Attach(entity);
            if (pros.Any())
            {
                foreach (var pro in pros)
                {
                    DbContext.Entry(entity).Property(pro).IsModified = true;
                }
                return await DbContext.SaveChangesAsync() == 1;
            }
            return false;
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

        public List<T> ExecProcudure<T>(string proName, MySqlParameter[] mySqlParameters)where T : class,new() 
        {
            var connection = DbContext.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                DbContext.Database.OpenConnection();
                cmd.CommandText = proName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(mySqlParameters);
                var dr = cmd.ExecuteReader();

                var data = new List<T>();
                //获取存储过程返回的单行值
                while (dr.Read())
                {
                    T item = new T();
                    Type type = item.GetType();
                    foreach (var propertyInfo in type.GetProperties())
                    {
                        //注意需要转换数据库中的DBNull类型
                        //获取存储过程返回的每一行里的每一列值
                        var value = dr.GetValue(dr.GetOrdinal(propertyInfo.Name));
                        propertyInfo.SetValue(item, value);
                        data.Add(item);
                    }
                }
                dr.Dispose();
                return data;
            }
        }
    }
}
