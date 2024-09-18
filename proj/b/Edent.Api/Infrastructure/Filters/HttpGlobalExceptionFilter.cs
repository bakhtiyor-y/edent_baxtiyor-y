using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Edent.Api.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;
        public HttpGlobalExceptionFilter(IHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var json = new JsonErrorResponse();

            if (env.IsDevelopment())
            {
                json.Messages = new[] { context.Exception.Message };
                json.DeveloperMessage = context.Exception;
            }
            else
            {
                json.Messages = new[] { context.Exception.Message };
            }

            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.ExceptionHandled = true;
        }
    }
}
