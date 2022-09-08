using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing.Attributes
{
    public class ModelValidatorAsync : ActionFilterAttribute
    {
        //===========================================================================
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Task.Run(() =>
           {
               if (!context.ModelState.IsValid)
               {
                   var errorMessages = string.Empty;
                   var errors = context.ModelState.Where(c => c.Value.ValidationState == ModelValidationState.Invalid).ToList();
                   foreach (var error in errors)
                       errorMessages += error.Value.Errors[0].ErrorMessage + "\n";
                   context.Result = new BadRequestObjectResult(errorMessages);
               }
               return base.OnActionExecutionAsync(context, next);
               //  return new Exception(MessageResource.InvalidModelState);

           });
        }
        //=============================================================================
    }
}
