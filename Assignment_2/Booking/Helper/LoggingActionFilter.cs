using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Log user activity details before the action executes
        var controllerName = context.RouteData.Values["controller"];
        var actionName = context.RouteData.Values["action"];
        var queryString = context.HttpContext.Request.QueryString;

        _logger.LogInformation($"User is accessing {controllerName}/{actionName} with query: {queryString}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Log additional details after the action executes
        var controllerName = context.RouteData.Values["controller"];
        var actionName = context.RouteData.Values["action"];
        var result = context.Result;

        _logger.LogInformation($"User accessed {controllerName}/{actionName}. Result: {result}");
    }
}
