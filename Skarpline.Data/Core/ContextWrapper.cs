using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Skarpline.Entities.Domain.Identity;
using Skarpline.Entities.Domain.Messages;

namespace Skarpline.Data.Core
{
    /// <summary>
    /// Wrapper for Db Context
    /// </summary>
    public class ContextWrapper : IdentityDbContext<User>, IContext
    {
        public ContextWrapper()
        {
        }

        public ContextWrapper(string connectionString) : base(connectionString)
        {
        }

        public IDbSet<Message> Messages { get; set; }
    }
}
