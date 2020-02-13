using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Linq;

namespace WorldJourney.Filters
{
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IHostingEnvironment _environment;
        private readonly string _contentRootPath;
        private readonly string _logPath;
        private readonly string _fileName;
        private readonly string _fullPath;

        public LogActionFilterAttribute(IHostingEnvironment environment)
        {
            _environment = environment;
            _contentRootPath = _environment.ContentRootPath;
            _logPath = _contentRootPath + "\\LogFile\\";
            _fileName = $"log {DateTime.Now:MM-dd-yyyy-H-mm}.txt";
            _fullPath = _logPath + _fileName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Directory.CreateDirectory(_logPath);
            var actionName = filterContext.ActionDescriptor.RouteValues["action"];
            var controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            using (var fs = new FileStream(_fullPath, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"The action {actionName} in {controllerName} controller started, event fired: OnActionExecuting");
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.RouteValues["action"];
            var controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            using (var fs = new FileStream(_fullPath, FileMode.Append))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"The action {actionName} in {controllerName} controller finished, event fired: OnActionExecuted");
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.RouteValues["action"];
            var controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            var result = (ViewResult)filterContext.Result;
            using (var fs = new FileStream(_fullPath, FileMode.Append))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"The action {actionName} in {controllerName} controller has the following viewData : {result.ViewData.Values.FirstOrDefault()}, event fired: OnResultExecuted");
                }
            }
        }
    }
}
