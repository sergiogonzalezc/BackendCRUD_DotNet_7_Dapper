using BackendCRUD.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Querys
{
    public class GetMemberByIdQuerys : IRequest<MemberDTO>
    {
        public int id { get; }

        public GetMemberByIdQuerys(int id)
        {
            this.id = id;
        }

    }
}
