using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BackendCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorBaseController : BackendCRUD.Api.Model.Controller
    {
        [Route("/errorBase")]
        [HttpGet]
        public IActionResult ErrorBase()
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();
                        
            try
            {
                if (context != null)
                {

                    return Problem(
                        detail: context.Error.StackTrace,
                        title: context.Error.Message);
                }
            }
            catch (Exception ex)
            {
            
            }

            return null;
        }
    }
}
