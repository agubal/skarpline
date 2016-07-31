using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Skarpline.Data.Core
{
    /// <summary>
    /// IRepository interface
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Saves changes in database
        /// </summary>
        void Save();

        /// <summary>
        /// Saves changes in database in async way
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }

    /// <summary>
    /// Generic IRepository interface
    /// </summary>
    /// <typeparam name="T">Type of repository</typeparam>
    public interface IRepository<T> : IRepository where T : class
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        IQueryable<T> All(string[] includes = null);

        /// <summary>
        /// Find by condition
        /// </summary>
        /// <param name="expression">Condition</param>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> expression, string[] includes = null);

        /// <summary>
        /// Find By Condition
        /// </summary>
        /// <param name="expression">Condition</param>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> expression, string[] includes = null);

        /// <summary>
        /// Find by Key
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns></returns>
        T FindByKey(object key);

        /// <summary>
        /// Filters by condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null);

        /// <summary>
        /// Creates new Entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns></returns>
        T Create(T entity);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
    }
}
