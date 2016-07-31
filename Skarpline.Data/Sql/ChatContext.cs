using System.Configuration;
using System.Data.Entity;
using Skarpline.Data.Core;

namespace Skarpline.Data.Sql
{
    public class ChatContext : ContextWrapper
    {
        static ChatContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ChatContext>());
            Database.SetInitializer<ChatContext>(null);
        }

        public ChatContext() : base(ConfigurationManager.ConnectionStrings["ChatContext"].ConnectionString) { }

        public ChatContext(string connectionString) : base(connectionString) { }

        public static ChatContext Create()
        {
            return new ChatContext();
        }
    }
}
