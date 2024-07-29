using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Handlers.GetMember
{
    /// <summary>
    /// Implement a CQRS handler 
    /// </summary>
    public class GetMemberHandler : IRequestHandler<GetMembersQuerys, List<MemberDTO>>
    {
        private readonly IMemberApplication _MembersService;

        public GetMemberHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<List<MemberDTO>> Handle(GetMembersQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetMembers();
        }

    }
}
