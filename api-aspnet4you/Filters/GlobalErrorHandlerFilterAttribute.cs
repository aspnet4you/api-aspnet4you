using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace api.aspnet4you.mvc5.Filters
{
    public class GlobalErrorHandlerFilterAttribute :  ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //FancyLogger.log.Error("Unhandled Exception.", actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}