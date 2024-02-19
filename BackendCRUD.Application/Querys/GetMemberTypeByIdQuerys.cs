using BackendCRUD.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Querys
{
    public record GetMemberTypeByIdQuerys : IRequest<MemberType>
    {
        public string id { get; }

        public GetMemberTypeByIdQuerys(string id)
        {
            this.id = id;
        }
    }
}
