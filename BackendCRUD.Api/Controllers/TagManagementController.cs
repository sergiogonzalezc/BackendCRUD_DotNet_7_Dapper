using Microsoft.AspNetCore.Mvc;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Interface;
using BackendCRUD.Api.Model;
using Microsoft.AspNetCore.Authorization;
using BackendCRUD.Common;
using MediatR;
using BackendCRUD.Application.Querys;
using Microsoft.AspNetCore.Cors;
using BackendCRUD.Application.Commands;

namespace BackendCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class TagManagementController : ControllerBase
    {
        private readonly IMemberApplication _MembersService;
        private readonly ILogger<TagManagementController> _logger;
        private readonly IMediator _mediator;

        private BackendCRUD.Api.Model.Error err = new BackendCRUD.Api.Model.Error
        {
            Code = StatusCodes.Status400BadRequest
        };

        public TagManagementController(IMemberApplication MembersServices, ILogger<TagManagementController> logger, IMediator mediator)
        {
            _MembersService = MembersServices;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Get the full tag list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tags")]
        [ProducesResponseType(typeof(List<BackendCRUD.Application.Model.Member>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<TagModel> GetTags()
        {
            string nameMethod = nameof(GetTags);
            try
            {
                _logger.LogInformation("Start...");

                // Implement a CQRS for query/command responsibility segregation
                var query = new GetTagQuerys();
                List<Tag> result = await _mediator.Send(query);

                var finalResult = new TagModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = result.ToList(),
                    Code = "0",
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Code = StatusCodes.Status206PartialContent;
                err.Message = arEx.Message;
                err.AditionalData = arEx.ParamName;

                return new TagModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Message = ex.Message;
                err.AditionalData = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new TagModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        
    }
}
