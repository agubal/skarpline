using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skarpline.Common.Results;

namespace Skarpline.Services
{
    /// <summary>
    /// Common Sets of methods for Business logic
    /// </summary>
    public interface IService : IDisposable
    {
    }

    /// <summary>
    /// Common Sets of generic methods for Business logic
    /// </summary>
    public interface IService<T> : IService where T : class
    {

        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<T> GetAll(string[] includes = null);

        /// <summary>
        /// Reterns entity by condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate, string[] includes = null);

        /// <summary>
        /// Reterns entity by condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[] includes = null);

        /// <summary>
        /// Reterns entity by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T GetByKey(object id);

        /// <summary>
        /// Reterns list of entities by condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="includes">List of children to include</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, string[] includes = null);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns></returns>
        Task<ServiceResult<T>> CreateAsync(T entity);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns></returns>
        ServiceResult<T> Create(T entity);

        /// <summary>
        /// Updates existed entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <returns></returns>
        Task<ServiceResult> UpdateAsync(T entity);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        Task<ServiceResult> DeleteAsync(T entity);
    }
}
