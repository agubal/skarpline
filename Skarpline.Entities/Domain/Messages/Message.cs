using System;
using Skarpline.Common.Identity;
using Skarpline.Entities.Domain.Identity;

namespace Skarpline.Entities.Domain.Messages
{
    /// <summary>
    /// Message database object
    /// </summary>
    public class Message : IIdentifier<int>
    {
        public string MessageBody { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public int Id { get; set; }
    }
}
