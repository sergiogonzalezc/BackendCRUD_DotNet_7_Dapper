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
    public class DeleteMemberHandler : IRequestHandler<DeleteMemberCommand, ResultRequestDTO>
    {
        private readonly IMemberApplication _MembersService;

        public DeleteMemberHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<ResultRequestDTO> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            return await _MembersService.DeleteMember(request.Id);
        }
    }
}

