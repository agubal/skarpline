using System.Web.Http;
using Skarpline.Entities.Domain.Messages;
using Skarpline.Entities.Models.Messages;
using Skarpline.Services;

namespace Skarpline.Api.Controllers.Messages
{
    /// <summary>
    /// Api controller to work with message object
    /// </summary>
    [Authorize, RoutePrefix("api/messages")]
    public class MessagesController : BaseApiController<Message, MessageModel, int>
    {
        public MessagesController(IService<Message> entityService) : base(entityService)
        {
        }
    }
}
