using System;
using Newtonsoft.Json;
using Skarpline.Common.Identity;

namespace Skarpline.Entities.Models.Messages
{
    /// <summary>
    /// Message view model
    /// </summary>
    public class MessageModel : IIdentifier<int>
    {
        [JsonProperty("message")]
        public string MessageBody { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("senderId")]
        public string UserId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        public int Id { get; set; }
    }
}
