using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalogo.Filters
{
    public class ApiExceptionFilters : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilters> _logger;

        public ApiExceptionFilters(ILogger<ApiExceptionFilters> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "It occurred an non treated excpetion");

            context.Result = new ObjectResult("An error occurred when processing your request")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}