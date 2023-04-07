using Blog.Api.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public int Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int Add(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public long Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public TEntity QueryById(object objId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> QueryByIds(object[] ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order = null, Expression<Func<TEntity, TEntity>> selection = null, bool isTracking = true)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
