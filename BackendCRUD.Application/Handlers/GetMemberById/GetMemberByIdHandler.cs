using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Handlers.GetMemberById
{

    /// <summary>
    /// Implement a CQRS handler 
    /// </summary>
    public class GetMemberByIdHandler : IRequestHandler<GetMemberByIdQuerys, MemberDTO>
    {
        private readonly IMemberApplication _MembersService;

        public GetMemberByIdHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<MemberDTO> Handle(GetMemberByIdQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetMembersById(request.id);
        }
    }
}
