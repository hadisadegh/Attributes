using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Attributes
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
                   context.Result = new BadRequestObjectResult(new SysResult<string>() { IsSuccess = false, Message = errorMessages, Value = null });
               }
               return base.OnActionExecutionAsync(context, next);
           });
        }
        //=============================================================================
    }
}
