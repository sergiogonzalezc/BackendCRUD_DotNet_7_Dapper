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
    public class MemberTagManagementController : ControllerBase
    {
        private readonly IMemberApplication _MembersService;
        private readonly ILogger<MemberTagManagementController> _logger;
        private readonly IMediator _mediator;

        private BackendCRUD.Api.Model.Error err = new BackendCRUD.Api.Model.Error
        {
            Code = StatusCodes.Status400BadRequest
        };

        public MemberTagManagementController(IMemberApplication MembersServices, ILogger<MemberTagManagementController> logger, IMediator mediator)
        {
            _MembersService = MembersServices;
            _logger = logger;
            _mediator = mediator;
        }

       
        /// <summary>
        /// Insert a new tag for a member
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InsertRoleTypeModel), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<InsertMemberTagModel> InsertMemberTag([FromBody] InputCreateMemberTag input)
        {
            string nameMethod = nameof(InsertMemberTag);
            try
            {
                // Implement a CQRS for query/command responsibility segregation

                var query = new InsertMemberTagCommand(input);
                ResultRequestDTO result = await _mediator.Send(query);

                _logger.LogInformation($"output: {result}");

                InsertMemberTagModel finalResult = null;

                if (result == null)
                {
                    finalResult = new InsertMemberTagModel
                    {
                        Status = Common.Enum.EnumMessage.Succes.ToString(),
                        SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                        Success = (result == null ? false : result.Success),
                        Message = (result == null ? "error" : (result?.ErrorMessage == null ? "" : result?.ErrorMessage)),
                        Data = null,
                        DataList = null,
                        Code = "500",
                    };
                }
                if (!result.Success)
                {
                    finalResult = new InsertMemberTagModel
                    {
                        Status = Common.Enum.EnumMessage.Succes.ToString(),
                        SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                        Success = (result == null ? false : result.Success),
                        Message = (result == null ? "error" : (result?.ErrorMessage == null ? "" : result?.ErrorMessage)),
                        Data = null,
                        DataList = null,
                    };
                }
                else
                {
                    finalResult = new InsertMemberTagModel
                    {
                        Status = Common.Enum.EnumMessage.Succes.ToString(),
                        SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                        Success = result.Success,
                        Message = result.ErrorMessage,
                        Data = result,
                        DataList = null,
                    };
                }

                return finalResult;
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Code = StatusCodes.Status206PartialContent;
                err.Message = arEx.Message;
                err.AditionalData = arEx.ParamName;

                return new InsertMemberTagModel
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

                return new InsertMemberTagModel
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
