using BackendCRUD.Application.Common;
using BackendCRUD.Application.CQRS;
using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BackendCRUD.Application.Handlers.GetMember
{
    public record GetMembersWithPaginationQuery : IRequest<List<MemberDTO>>
    {    
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }
    }

    //public record GetMembersQuerys(int? PageNumber = 1, int? PageSize = 10) : IQuery<List<MemberDTO>>;
    //public record GetMembresResult(IEnumerable<MemberDTO> Members);

    /// <summary>
    /// Implement a CQRS handler 
    /// </summary>
    public class GetMemberHandler : IRequestHandler<GetMembersWithPaginationQuery, List<MemberDTO>>
    {
        private readonly IMemberApplication _MembersService;

        public GetMemberHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        /// <summary>
        /// Obtiene la lista completa de miembros
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<MemberDTO>> Handle(GetMembersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetMembers(request.PageNumber ?? 1, request.PageSize ?? 10);
        }

    }
}
