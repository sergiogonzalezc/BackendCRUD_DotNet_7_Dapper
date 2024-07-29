using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Handlers.GetTag
{
    /// <summary>
    /// Implement a CQRS handler for Tag
    /// </summary>
    public class GetTagHandler : IRequestHandler<GetTagQuerys, List<Tag>>
    {
        private readonly IMemberApplication _MembersService;

        public GetTagHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<List<Tag>> Handle(GetTagQuerys request, CancellationToken cancellationToken)
        {
            return await _MembersService.GetTags();
        }

    }
}
