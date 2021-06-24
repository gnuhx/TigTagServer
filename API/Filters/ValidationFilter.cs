using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ValidationFilter:IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(k => k.Key, k => k.Value.Errors.Select(x=>x.ErrorMessage)).ToArray();
                var errors = new List<string>();
                foreach (var error in errorsInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        errors.Add(subError);
                    }
                }
                var dataResult = new
                {
                    Status = false,
                    ErrorMessages = errors
                };
                context.Result = new BadRequestObjectResult(dataResult);
                return;
            }

            await next();
        }
    }
}
