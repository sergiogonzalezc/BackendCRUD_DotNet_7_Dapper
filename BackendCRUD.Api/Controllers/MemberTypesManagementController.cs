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
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure;

namespace BackendCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class MemberTypesManagementController : ControllerBase
    {
        private readonly IMemberApplication _MembersService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private BackendCRUD.Api.Model.Error err = new BackendCRUD.Api.Model.Error
        {
            Code = StatusCodes.Status400BadRequest
        };

        public MemberTypesManagementController(IMemberApplication MembersServices, IMediator mediator, IMapper mapper)
        {
            _MembersService = MembersServices;
            _mediator = mediator;
            _mapper = mapper;
        }


        /// <summary>
        /// Get the list for the Member types
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetMemberTypes")]
        [Route("membersTypes")]
        [ProducesResponseType(typeof(List<BackendCRUD.Application.Model.MemberType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<MemberTypesModel> GetMemberTypes()
        {
            string nameMethod = nameof(GetMemberTypes);
            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMemberTypesQuerys();
                List<Application.Model.MemberType> result = await _mediator.Send(query);

                var finalResult = new MemberTypesModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = result,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Code = StatusCodes.Status206PartialContent;
                err.Message = arEx.Message;
                err.AditionalData = arEx.ParamName;

                return new MemberTypesModel
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

                return new MemberTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Get the Member type List by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetMemberTypeById")]
        [Route("membersTypes/{id}")]
        [ProducesResponseType(typeof(BackendCRUD.Application.Model.MemberType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<MemberTypesModel> GetMemberTypeById(string id)
        {
            string nameMethod = nameof(GetMemberTypes);
            MemberTypesModel finalResult = new MemberTypesModel();

            try
            {
                //if (string.IsNullOrEmpty(id))
                //{
                //    finalResult = new MemberTypesModel
                //    {
                //        Status = Common.Enum.EnumMessage.Error.ToString(),
                //        SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                //        Success = true,
                //        Message = "not found",
                //        Data = null,
                //        Code = "999",
                //    };
                //    return finalResult;
                //}


                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMemberTypeByIdQuerys(id);
                Application.Model.MemberType result = await _mediator.Send(query);

                finalResult = _mapper.Map<MemberTypesModel>(result);
                finalResult.Success = true;
                finalResult.Data = result;
                
                /***
                var authResult = await _mediator.Send(query);

                var response = _mapper.Map<GetTransactionsByAccIdResultResponse>(authResult);

                return Ok(response);

                var data = await _unitOfWork.Contacts.GetByIdAsync(id);
                apiResponse.Success = true;
                apiResponse.Result = data;
                */

                // If ID not found
                //if (result == null)
                //{
                //    finalResult = new MemberTypesModel
                //    {
                //        Status = Common.Enum.EnumMessage.Succes.ToString(),
                //        SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                //        Success = true,
                //        Message = "not found",
                //        Data = result,
                //        Code = "404",
                //    };
                //}
                //else
                //{
                //    finalResult = new MemberTypesModel
                //    {
                //        Status = Common.Enum.EnumMessage.Succes.ToString(),
                //        SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                //        Success = true,
                //        Message = "ok",
                //        Data = result,
                //        Code = "0",
                //    };
                //}
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                //err.Code = StatusCodes.Status206PartialContent;
                //err.Message = arEx.Message;
                //err.AditionalData = arEx.ParamName;

                //return new MemberTypesModel
                //{
                //    Status = Common.Enum.EnumMessage.Error.ToString(),
                //    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                //    Success = false,
                //    Message = arEx.Message
                //};

                finalResult.Success = false;
                finalResult.Message = arEx.Message;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                //err.Message = ex.Message;
                //err.AditionalData = ex.GetBaseException().Message;
                ////return BadRequest(err);

                //return new MemberTypesModel
                //{
                //    Status = Common.Enum.EnumMessage.Error.ToString(),
                //    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                //    Success = false,
                //    Message = ex.Message
                //};

                finalResult.Success = false;
                finalResult.Message = ex.Message;
            }

            return finalResult;
        }

    }
}
