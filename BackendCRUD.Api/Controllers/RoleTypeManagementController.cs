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
    public class RoleTypeManagementController : ControllerBase
    {
        private readonly IMemberApplication _MembersService;
        private readonly ILogger<RoleTypeManagementController> _logger;
        private readonly IMediator _mediator;

        private BackendCRUD.Api.Model.Error err = new BackendCRUD.Api.Model.Error
        {
            Code = StatusCodes.Status400BadRequest
        };

        public RoleTypeManagementController(IMemberApplication MembersServices, ILogger<RoleTypeManagementController> logger, IMediator mediator)
        {
            _MembersService = MembersServices;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Get the full role type list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("roleTypes")]
        [ProducesResponseType(typeof(List<BackendCRUD.Application.Model.Member>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<RoleTypeModel> GetRoleTypes()
        {
            string nameMethod = nameof(GetRoleTypes);
            try
            {
                _logger.LogInformation("Start...");

                // Implement a CQRS for query/command responsibility segregation
                var query = new GetRoleTypeQuerys();
                List<RoleType> result = await _mediator.Send(query);

                var finalResult = new RoleTypeModel
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

                return new RoleTypeModel
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

                return new RoleTypeModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Insert a new role type
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InsertRoleTypeModel), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<InsertRoleTypeModel> InsertRoleType([FromBody] InputCreateRoleType input)
        {
            string nameMethod = nameof(InsertRoleType);
            try
            {
                // Implement a CQRS for query/command responsibility segregation

                var query = new InsertRoleTypeCommand(input);
                ResultRequestDTO result = await _mediator.Send(query);

                _logger.LogInformation($"output: {result}");

                InsertRoleTypeModel finalResult = null;

                if (result == null)
                {
                    finalResult = new InsertRoleTypeModel
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
                    finalResult = new InsertRoleTypeModel
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
                    finalResult = new InsertRoleTypeModel
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

                return new InsertRoleTypeModel
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

                return new InsertRoleTypeModel
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
