﻿using AutoMapper;
using BackendCRUD.Application.Commands;
using BackendCRUD.Application.Handlers;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using BackendCRUD.Common;
using BackendCRUD.Minimal.Api.Model;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BackendCRUD.Minimal.Api.Endpoints
{
    public class MemberManagementEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/members");

            group.MapPost("", InsertMember);
            group.MapGet("", GetMembers);
            group.MapGet("{id}", GetMember).WithName(nameof(GetMember));
            group.MapPut("{id}", UpdateMember).WithName(nameof(UpdateMember));
            group.MapDelete("{id}", DeleteMember).WithName(nameof(DeleteMember));
        }


        /// <summary>
        /// Get the member list
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("members")]
        //[ProducesResponseType(typeof(List<BackendCRUD.Application.Model.Member>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[AllowAnonymous]
        public static async Task<Results<Ok<MemberModel>, BadRequest<MemberModel>>> GetMembers(ISender mediator)
        {
            string nameMethod = nameof(GetMembers);
            MemberModel finalResult = new MemberModel();

            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMembersQuerys();
                List<MemberDTO> result = await mediator.Send(query);

                finalResult.Success = true;
                finalResult.DataList = result;

            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return TypedResults.BadRequest(finalResult);
            }

            return TypedResults.Ok(finalResult);

        }


        /// <summary>
        /// Get a unique member by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet]
        ////[Route("GetMemberById")]
        ////[Route("members/{memberId}/orders")]
        //[Route("members/{id}")]
        //[ProducesResponseType(typeof(BackendCRUD.Application.Model.Member), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[AllowAnonymous]
        public static async Task<IResult> GetMember(int id, ISender mediator)
        {
            string nameMethod = nameof(GetMembers);
            MemberModel finalResult = new();

            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetMemberByIdQuerys(id);
                MemberDTO result = await mediator.Send(query);
                
                
                //var response = mapper.Map<AuthenticationResponse>(result);

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
                return TypedResults.BadRequest(finalResult);
            }

            return TypedResults.Ok(finalResult);
        }


        /// <summary>
        /// Insert a new member
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("insert")]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(InsertMemberModel), StatusCodes.Status200OK)]
        //[AllowAnonymous]
        public static async Task<IResult> InsertMember([FromBody] InputCreateMember input, ISender mediator)
        {
            string nameMethod = nameof(InsertMember);
            InsertMemberModel finalResult = new();

            try
            {
                // Implement a CQRS for query/command responsibility segregation

                var query = new InsertMemberCommand(input);
                ResultRequestDTO result = await mediator.Send(query);

                finalResult.Success = true;
                finalResult.Data = result;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return TypedResults.BadRequest(finalResult);
            }

            return TypedResults.Ok(finalResult);
        }


        /// <summary>
        /// Update a specific member by Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPut]
        //[Route("update")]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[AllowAnonymous]
        public static async Task<IResult> UpdateMember([FromBody] InputUpdateMember input, ISender mediator)
        {
            string nameMethod = nameof(UpdateMember);
            UpdateMemberModel finalResult = new();
            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new UpdateMemberCommand(input);
                ResultRequestDTO result = await mediator.Send(query);

                finalResult.Success = true;
                finalResult.Data = result;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return TypedResults.BadRequest(finalResult);
            }

            return TypedResults.Ok(finalResult);
        }


        /// <summary>
        /// Delete a specific member by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //[HttpDelete]
        //[Route("delete")]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[AllowAnonymous]
        public static async Task<IResult> DeleteMember(int Id, ISender mediator)
        {
            string nameMethod = nameof(DeleteMember);
            DeleteMemberModel finalResult = new();
            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new DeleteMemberCommand(Id);
                ResultRequestDTO result = await mediator.Send(query);

                finalResult.Success = true;
                finalResult.Data = result;
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                finalResult.Success = false;
                finalResult.Message = ex.Message;
                return TypedResults.BadRequest(finalResult);
            }

            return TypedResults.Ok(finalResult);
        }
    }

}

