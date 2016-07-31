using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skarpline.Data.Core
{
    /// <summary>
    /// Implementation for Unit of work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<string, IRepository> _repositories;
        private readonly ContextWrapper _context;

        public UnitOfWork(IContext context)
        {
            _repositories = new Dictionary<string, IRepository>();
            _context = context as ContextWrapper;
        }

        public void Register(IRepository repository)
        {
            _repositories.Add(repository.GetType().ToString(), repository);
        }

        public async Task SaveAsync()
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                foreach (var repo in _repositories)
                {
                    await repo.Value.SaveAsync();
                }

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }

        }

        public void Save()
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                foreach (var repo in _repositories)
                {
                    repo.Value.Save();
                }

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            foreach (var repo in _repositories)
            {
                repo.Value.Dispose();
            }
        }
    }
}
