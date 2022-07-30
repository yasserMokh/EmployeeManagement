using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {

        [Route("Error/{statusCode}")]
        public IActionResult StatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page is not found!";
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            //Retreive exception details
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View();
        }
    }
}
