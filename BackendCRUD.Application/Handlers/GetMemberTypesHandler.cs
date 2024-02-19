using BackendCRUD.Application.Commands;
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
    public class GetMemberTypesHandler : IRequestHandler<GetMemberTypesQuerys, List<MemberType>>
    {
        private readonly IMemberApplication _MembersService;

        public GetMemberTypesHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<List<MemberType>> Handle(GetMemberTypesQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetMemberTypes();
        }
    }
}