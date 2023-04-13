using Blog.Api.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Blog.Api.IServices
{
    /// <summary>
    /// 数据库操作的通用接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseServices<TEntity> where TEntity : class,new()
    {
        /// <summary>
        /// 数据库上下文对象
        /// </summary>
        public BlogsqlContext DbContext { get; set; }

        /// <summary>
        /// 事务操作
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; set; }

        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryById(object objId);

        /// <summary>
        /// 根据条件查询实体集合
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <param name="selection">投影表达式</param>
        /// <param name="isTracking">是否跟踪实体</param>
        /// <returns></returns>
        IQueryable<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order = null,
            Expression<Func<TEntity, TEntity>> selection = null, bool isTracking = true);

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> QueryAll();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page">查询的页面</param>
        /// <param name="pageCount">每页几条数据</param>
        /// <returns></returns>
        IQueryable<TEntity> QueryPage(int pageIndex, int pageCount);

       
        /// <summary>
        /// 获取实体数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<long> Count(Expression<Func<TEntity,bool>>predicate=null);

        /// <summary>
        /// 实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 添加单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>影响行数</returns>
        Task<int> Add(TEntity entity);

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>影响行数</returns>
        Task<int> Add(List<TEntity> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> Update(List<TEntity> entities);

       /// <summary>
       /// 删除单个实体
       /// </summary>
       /// <param name="entity"></param>
       /// <returns></returns>
        Task<bool> Delete(TEntity entity);

        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> Delete(List<TEntity> entities);

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();
        
        /// <summary>
        /// 关闭事务
        /// </summary>
        void Commit();
    }
}
