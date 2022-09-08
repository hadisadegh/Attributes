using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ticketing.Controllers;
using Ticketing.Helpers;
using Ticketing.Services;


namespace Ticketing.Attributes
{
    public class ExceptionHandlerAsync : ExceptionFilterAttribute
    {

        public string DefaultErrorMessage { get; set; }


        //=============================================================================
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await Task.Run(() =>
            {
                // var currIdtUser = _unitOfWork.IdtUser.GetCurrentUserAsync();
                var requestInfo = CommonFunc.GetRequestInfo();
                var messeageStructure = context.Exception.Message.ToString().Split('_');

                string code = null;
                string controllerName = null;
                string actionName = null;
                string message = (context.Exception.InnerException == null) ? messeageStructure[0] : (context.Exception.InnerException.Message + " line:" + context.Exception.InnerException.LineNumber());
                string date = PersianDateTime.GetJalaliStrDateTime(DateTime.Now);
                string line = null;
                string inputParameters = null;
                string inputData = null;
                string outputObjects = null;
                string outputData = null;
                // string currentUserId = currIdtUser.Result != null ? currIdtUser.Result.UserId.ToString() : null;
                string deviceFullName = requestInfo.Device;
                string device = requestInfo.Device.Contains("Windows") ? "Windows Browser" :
                                requestInfo.Device.Contains("Android") ? "Android Browser" :
                                requestInfo.Device.Contains("iPhone") ? "iPhone Browser" :
                                requestInfo.Device.Contains("okhttp") ? "Android App" : null;
                if (messeageStructure.Length == 9)
                {
                    code = messeageStructure[1];
                    controllerName = messeageStructure[2];
                    actionName = messeageStructure[3];
                    line = messeageStructure[4];
                    inputParameters = messeageStructure[5];
                    inputData = messeageStructure[6];
                    outputObjects = messeageStructure[7];
                    outputData = messeageStructure[8];
                }
                else
                {
                    //controllerName = controllerName.Split('\\').Last().Split('.').First();
                    line = (context.Exception.InnerException != null) ? context.Exception.InnerException.LineNumber().ToString() : "0";
                }


                return new BaseController(null).HandleError(context.Exception);

                //throw new Exception();
            });
        }
        //=============================================================================

    }



}
