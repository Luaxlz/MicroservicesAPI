using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommandsService.Filters
{
    public class ValidatePlatformExistsFilter : IActionFilter
    {
        private readonly ICommandRepo _repository;

        public ValidatePlatformExistsFilter(ICommandRepo repository)
        {
            _repository = repository;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("platformId", out var platformIdObj) && 
                platformIdObj is int platformId)
            {
                Console.WriteLine($"--> Validating platform exists: {platformId}");
                
                if (!_repository.PlatformExists(platformId))
                {
                    Console.WriteLine($"--> Platform {platformId} not found");
                    context.Result = new NotFoundObjectResult($"Platform {platformId} not found");
                    return; // Para a execução aqui
                }
                
                Console.WriteLine($"--> Platform {platformId} exists, continuing...");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}