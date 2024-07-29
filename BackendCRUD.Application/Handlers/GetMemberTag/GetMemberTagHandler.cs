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

namespace BackendCRUD.Application.Handlers.GetMemberTag
{
    public class GetMemberTagHandler : IRequestHandler<GetMemberTagQuerys, List<MemberTagDTO>>
    {
        private readonly IMemberApplication _MembersService;

        public GetMemberTagHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<List<MemberTagDTO>> Handle(GetMemberTagQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetMemberTags();
        }
    }
}