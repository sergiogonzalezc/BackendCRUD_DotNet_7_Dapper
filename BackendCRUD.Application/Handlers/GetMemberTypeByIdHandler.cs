using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Handlers
{
    public class GetMemberTypeByIdHandler : IRequestHandler<GetMemberTypeByIdQuerys, MemberType>
    {
        private readonly IMemberApplication _MembersService;

        public GetMemberTypeByIdHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<MemberType> Handle(GetMemberTypeByIdQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetMemberTypeById(request.id);
        }
    }
}