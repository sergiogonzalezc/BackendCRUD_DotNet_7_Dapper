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
    /// <summary>
    /// Implement a CQRS handler for RoleType
    /// </summary>
    public class GetRoleTypeHandler : IRequestHandler<GetRoleTypeQuerys, List<RoleType>>
    {
        private readonly IMemberApplication _MembersService;

        public GetRoleTypeHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<List<RoleType>> Handle(GetRoleTypeQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetRoleTypes();
        }

    }
}
