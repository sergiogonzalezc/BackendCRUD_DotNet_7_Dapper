using BackendCRUD.Application.Commands;
using BackendCRUD.Application.Interface;
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
    /// Validate a Member by name and last name
    /// </summary>
    public class GetValidateMemberHandler : IRequestHandler<GetValidateMemberQuery, bool>
    {
        private readonly IMemberApplication _MembersService;

        public GetValidateMemberHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<bool> Handle(GetValidateMemberQuery request, CancellationToken cancellationToken)
        {
            return await _MembersService.ExistsMemberByName(request.name);
        }

    }
}
