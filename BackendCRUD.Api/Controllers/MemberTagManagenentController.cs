using Microsoft.AspNetCore.Mvc;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Interface;
using BackendCRUD.Api.Model;
using Microsoft.AspNetCore.Authorization;
using BackendCRUD.Common;
using MediatR;
using BackendCRUD.Application.Querys;
using Microsoft.AspNetCore.Cors;

namespace BackendCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class MemberTagManagenentController : ControllerBase
    {
        private readonly IMemberApplication _MembersService;
        private readonly ILogger<MemberTagManagenentController> _logger;
        private readonly IMediator _mediator;

        private BackendCRUD.Api.Model.Error err = new BackendCRUD.Api.Model.Error
        {
            Code = StatusCodes.Status400BadRequest
        };

        public MemberTagManagenentController(IMemberApplication MembersServices, ILogger<MemberTagManagenentController> logger, IMediator mediator)
        {
            _MembersService = MembersServices;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Get the full member tag list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("membersTag")]
        [ProducesResponseType(typeof(List<BackendCRUD.Application.Model.Member>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<MemberTagModel> GetMemberTags()
        {
            string nameMethod = nameof(GetMemberTags);
            try
            {
                _logger.LogInformation("Start...");

                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMemberTagQuerys();
                List<MemberTagDTO> result = await _mediator.Send(query);

                var finalResult = new MemberTagModel
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

                return new MemberTagModel
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

                return new MemberTagModel
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
