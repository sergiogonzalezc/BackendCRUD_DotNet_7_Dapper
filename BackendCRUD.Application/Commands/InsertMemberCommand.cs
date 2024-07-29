using BackendCRUD.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackendCRUD.Application.CQRS;

namespace BackendCRUD.Application.Commands
{
    public record InsertMemberCommand(InputCreateMember input) : ICommand<ResultRequestDTO>
    {
    }
}
