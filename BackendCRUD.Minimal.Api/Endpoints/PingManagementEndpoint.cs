using AutoMapper;
using BackendCRUD.Application.Commands;
using BackendCRUD.Application.Handlers;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using BackendCRUD.Common;
using BackendCRUD.Minimal.Api.Model;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackendCRUD.Minimal.Api.Endpoints
{
    [AllowAnonymous]
    public class PingManagementEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/ping");

            group.MapGet("", GetPing);
        }

        [HttpGet]
        [AllowAnonymous]
        public static async Task<Results<Ok<MemberModel>, BadRequest<MemberModel>>> GetPing()
        {
            MemberModel finalResult = new MemberModel();
            return TypedResults.Ok(new MemberModel() { Message = "Ping ok" });
        }
    }

}

