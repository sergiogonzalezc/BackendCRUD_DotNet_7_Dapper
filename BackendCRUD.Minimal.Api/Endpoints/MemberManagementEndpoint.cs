using AutoMapper;
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
using System.Net;
using FluentValidation;
using BackendCRUD.Application.Common;
using BackendCRUD.Application.Handlers.GetMember;

namespace BackendCRUD.Minimal.Api.Endpoints
{
    //public record GetMembersWithPaginationQuery(int? PageNumber = 1, int? PageSize = 10);

    public class MemberManagementEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/members").WithOpenApi();

            group.MapPost("", InsertMember)
                        .Produces<InsertMemberModel>(StatusCodes.Status201Created)
                        .ProducesProblem(StatusCodes.Status400BadRequest)
                        .WithSummary("Inserta un nuevo miembro")
                        .WithDescription("Demo");

            group.MapGet("", GetMembers)
                        //.Produces<Results<Ok<MemberModel>, BadRequest<MemberModel>>>(StatusCodes.Status200OK)
                        //.ProducesProblem(StatusCodes.Status400BadRequest)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .WithSummary("Obtiene lista de miembros")
                        .WithDescription("Obtiene lista de miembros");

            group.MapGet("{id}", GetMember).WithName(nameof(GetMember))
                        .Produces<MemberModel>(StatusCodes.Status200OK)
                        .ProducesProblem(StatusCodes.Status400BadRequest)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .WithSummary("Obtiene lista de miembros por ID")
                        .WithDescription("Obtiene lista de miembros por ID");


            group.MapPut("{id}", UpdateMember).WithName(nameof(UpdateMember))
                        .Produces<UpdateMemberModel>(StatusCodes.Status200OK)
                        .ProducesProblem(StatusCodes.Status400BadRequest)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .WithSummary("Obtiene lista de miembros por ID")
                        .WithDescription("Obtiene lista de miembros por ID");

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
        public static async Task<List<MemberDTO>> GetMembers(ISender mediator, [AsParameters] GetMembersWithPaginationQuery request)
        {
            // Implement a CQRS for query/command responsibility segregation
            return await mediator.Send(request);
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

