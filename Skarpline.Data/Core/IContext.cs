using System.Data.Entity;
using Skarpline.Entities.Domain.Messages;

namespace Skarpline.Data.Core
{
    public interface IContext
    {
        IDbSet<Message> Messages { get; set; }
    }
}
