using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace SharedResources.Contracts
{
    public interface IRepository<out TContext, TEntity> where TEntity : class where TContext : DbContext
    {
        TContext GetContext { get; }

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);

        bool Create(TEntity entity);

        void Create(IEnumerable<TEntity> entities);

        bool Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        List<TEntity> ListAll();

        TEntity Read(Expression<Func<TEntity, bool>> predicate);
    }
}