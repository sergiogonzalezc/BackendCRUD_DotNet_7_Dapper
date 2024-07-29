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
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using BackendCRUD.Application.Handlers;

namespace BackendCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class MemberManagementController : ControllerBase
    {
        private readonly IMemberApplication _membersApplication;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private BackendCRUD.Api.Model.Error err = new BackendCRUD.Api.Model.Error
        {
            Code = StatusCodes.Status400BadRequest
        };

        public MemberManagementController(IMemberApplication MembersServices, IMediator mediator, IMapper mapper)
        {
            _membersApplication = MembersServices;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the member list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("members")]
        [ProducesResponseType(typeof(List<BackendCRUD.Application.Model.Member>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<MemberModel> GetMembers()
        {
            string nameMethod = nameof(GetMembers);
            MemberModel finalResult = new MemberModel();

            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMembersQuerys();
                List<MemberDTO> result = await _mediator.Send(query);

                finalResult.Success = true;
                finalResult.DataList = result;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = arEx.Message;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
            }

            return finalResult;

        }


        /// <summary>
        /// Get a unique member by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetMemberById")]
        //[Route("members/{memberId}/orders")]
        [Route("members/{id}")]
        [ProducesResponseType(typeof(BackendCRUD.Application.Model.Member), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetMember(int id)
        {
            string nameMethod = nameof(GetMember);
            MemberModel finalResult = new();

            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMemberByIdQuerys(id);
                MemberDTO result = await _mediator.Send(query);
                //var validator = new GetMemberByIdHandlerValidator();

                //var resultValidation = validator.Validate(result);

                //if (!resultValidation.IsValid)
                //    return Results.ValidationProblem(resultValidation.Errors());

                finalResult.Success = true;
                finalResult.Data = result;

            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return BadRequest(finalResult);
            }

            return Ok(finalResult);
        }


        /// <summary>
        /// Insert a new member
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InsertMemberModel), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> InsertMember([FromBody] InputCreateMember input)
        {
            string nameMethod = nameof(InsertMember);
            InsertMemberModel finalResult = new();

            try
            {
                // Implement a CQRS for query/command responsibility segregation

                var query = new InsertMemberCommand(input);
                ResultRequestDTO result = await _mediator.Send(query);

                finalResult.Success = true;
                finalResult.Data = result;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return BadRequest(finalResult);
            }

            return Ok(finalResult);
        }




        /// <summary>
        /// Update a specific member by Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateMember([FromBody] InputUpdateMember input)
        {
            string nameMethod = nameof(UpdateMember);
            UpdateMemberModel finalResult = new();
            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new UpdateMemberCommand(input);
                ResultRequestDTO result = await _mediator.Send(query);

                finalResult.Success = true;
                finalResult.Data = result;
            }

            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return BadRequest(finalResult);
            }

            return Ok(finalResult);
        }


        /// <summary>
        /// Delete a specific member by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMember([FromBody] int Id)
        {
            string nameMethod = nameof(DeleteMember);
            DeleteMemberModel finalResult = new();
            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new DeleteMemberCommand(Id);
                ResultRequestDTO result = await _mediator.Send(query);

                finalResult.Success = true;
                finalResult.Data = result;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Message = ex.Message;
                err.AditionalData = ex.GetBaseException().Message;
                return BadRequest(err);
            }

            return Ok(finalResult);
        }
    }
}
