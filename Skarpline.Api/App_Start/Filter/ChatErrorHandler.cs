using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Skarpline.Common.Results;
using Skarpline.Common.Utils;

namespace Skarpline.Api.Filter
{
    public class ChatErrorHandler : ExceptionFilterAttribute
    {
        /// <summary>
        /// Filter to catch all exceptions and wrap then into common response
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            string errorMessage = ErrorUtils.GetErrorMessage(context.Exception, "Service error");
            var response = new ServiceResult(errorMessage);

            context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, response);
            base.OnException(context);
        }
    }
}