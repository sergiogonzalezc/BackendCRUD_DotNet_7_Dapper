using Microsoft.AspNetCore.Mvc;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Interface;
using BackendCRUD.Api.Model;
using Microsoft.AspNetCore.Authorization;
using BackendCRUD.Common;
using MediatR;
using BackendCRUD.Application.Querys;
using Microsoft.AspNetCore.Cors;
using System.Net;

namespace BackendCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("GetPing")]
        public async Task<IActionResult> GetPing()
        {
            return Ok("ping ok");
        }
    }
}
