using System;
using System.Threading.Tasks;

namespace Skarpline.Data.Core
{
    /// <summary>
    /// Represent Unit of work for repository logic
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Registers repository
        /// </summary>
        /// <param name="repository"></param>
        void Register(IRepository repository);

        /// <summary>
        /// Saves changes after pool of actions
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        
        /// <summary>
        /// Saves changes after pool of actions
        /// </summary>
        /// <returns></returns>
        void Save();
    }
}
