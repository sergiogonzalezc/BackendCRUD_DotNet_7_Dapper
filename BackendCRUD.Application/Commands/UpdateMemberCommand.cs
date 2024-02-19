﻿using BackendCRUD.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Commands
{
    public record UpdateMemberCommand(InputUpdateMember input) : IRequest<ResultRequestDTO>
    {
    }
}
