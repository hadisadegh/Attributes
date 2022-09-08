
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Web.Attributes
{
    public class ExceptionHandlerAsync : ExceptionFilterAttribute
    {
        public string DefaultErrorMessage { get; set; }
        //=============================================================================
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await Task.Run(() =>
            {

                if (context.Exception.GetType().Name == "CustomException")
                {
                    context.Result = new BadRequestObjectResult(new SysResult<string>() { IsSuccess = false, Message = context.Exception.Message });
                }
                else
                {
                    if (!string.IsNullOrEmpty(DefaultErrorMessage))
                    {
                        context.Result = new BadRequestObjectResult(new SysResult<string>() { IsSuccess = false, Message = DefaultErrorMessage });
                    }
                    else
                    {
                        context.Result = new BadRequestObjectResult(new SysResult<string>() { IsSuccess = false, Message = context.Exception.Message });
                    }
                }
                return base.OnExceptionAsync(context);
            });
        }
        //=============================================================================

    }
}
