using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BackendCRUD.Application.Handlers
{

    /// <summary>
    /// Implement a CQRS handler 
    /// </summary>
    public sealed class GetMemberByIdHandlerValidator : AbstractValidator<MemberDTO>
    {
        public GetMemberByIdHandlerValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0).WithMessage("Error hhhh");
        }
    }
}
