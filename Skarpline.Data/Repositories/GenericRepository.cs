using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Skarpline.Data.Core;

namespace Skarpline.Data.Repositories
{
    /// <summary>
    /// Implementation of Generic IRepository interface
    /// </summary>
    public abstract class GenericRepository : IRepository
    {
        protected readonly ContextWrapper Context;
        private bool _disposed;

        protected GenericRepository(IUnitOfWork unitOfWork, IContext context)
        {
            unitOfWork.Register(this);
            Context = context as ContextWrapper;

            if (Context == null)
            {
                throw new NullReferenceException("Context is null");
            }
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                }
            }

            _disposed = true;
        }

    }

    /// <summary>
    /// Implementation for generic IRepository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : GenericRepository, IRepository<T> where T : class
    {
        protected readonly DbSet<T> DbSet;

        public GenericRepository(IUnitOfWork unitOfWork, IContext context)
            : base(unitOfWork, context)
        {
            DbSet = Context.Set<T>();
        }

        public virtual IQueryable<T> All(string[] includes = null)
        {
            if (includes == null || !includes.Any()) return DbSet.AsQueryable();

            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));

            var result = query.AsQueryable();
            return result;
        }

        public virtual T Find(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes == null || !includes.Any()) return DbSet.FirstOrDefault(predicate);

            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));

            var result = query.FirstOrDefault(predicate);
            return result;
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes == null || !includes.Any()) return DbSet.FirstOrDefault(predicate);

            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));

            var result = await query.FirstOrDefaultAsync(predicate);
            return result;
        }

        public virtual T FindByKey(object key)
        {
            return DbSet.Find(key);
        }

        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes == null || !includes.Any()) return DbSet.Where(predicate).AsQueryable();

            var query = DbSet.Include(includes.First());
            query = includes.Skip(1).Aggregate(query, (current, include) => current.Include(include));

            var result = query.Where(predicate).AsQueryable();
            return result;
        }

        public virtual T Create(T entity)
        {
            var result = DbSet.Add(entity);
            return result;
        }

        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
