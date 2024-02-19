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
    public class InsertRoleTypeHandler : IRequestHandler<InsertRoleTypeCommand, ResultRequestDTO>
    {
        private readonly IMemberApplication _MembersService;

        public InsertRoleTypeHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<ResultRequestDTO> Handle(InsertRoleTypeCommand request, CancellationToken cancellationToken)
        {
            return await _MembersService.InsertRoleType(request.input);
        }
    }
}

