using BackendCRUD.Application.Commands;
using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Handlers.UpdateMember
{
    public class UpdateMemberHandler : IRequestHandler<UpdateMemberCommand, ResultRequestDTO>
    {
        private readonly IMemberApplication _MembersService;

        public UpdateMemberHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<ResultRequestDTO> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            return await _MembersService.UpdateMember(request.input);
        }
    }
}