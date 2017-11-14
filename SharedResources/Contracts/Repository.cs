using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SharedResources.Contracts
{
    public abstract class Repository<TContext, TEntity> : IRepository<TContext, TEntity> where TEntity : class
        where TContext : DbContext
    {
        public TContext GetContext { get; }

        public int Count()
        {
            var num = _entitySet.Count();
            return num;
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _entitySet.Where(predicate).ToList().Count;
        }

        public bool Create(TEntity entity)
        {
            _entitySet.Add(entity);
            GetContext.SaveChanges();
            return _entitySet.AsEnumerable().Any(c => c == entity);
        }

        public void Create(IEnumerable<TEntity> entities)
        {
            _entitySet.AddRange(entities);
            GetContext.SaveChanges();
        }

        public bool Delete(TEntity entity)
        {
            _entitySet.Remove(entity);
            GetContext.SaveChanges();
            return _entitySet.AsEnumerable().All(c => c != entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            _entitySet.RemoveRange(entities);
            GetContext.SaveChanges();
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entitySet.Where(predicate).ToList();
        }

        public List<TEntity> ListAll()
        {
            return _entitySet.ToList();
        }

        public TEntity Read(Expression<Func<TEntity, bool>> predicate)
        {
            return _entitySet.SingleOrDefault(predicate);
        }

        protected Repository(TContext context)
        {
            GetContext = context;
            _entitySet = context.Set<TEntity>();
        }

        private readonly DbSet<TEntity> _entitySet;
    }
}