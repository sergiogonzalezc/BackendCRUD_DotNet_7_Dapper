using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackendCRUD.Application.Model;
using BackendCRUD.Infraestructure.Repository;

namespace BackendCRUD.Infraestructure.Repository
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MemberEF, Member>().ReverseMap();
            CreateMap<MemberTypesEF, MemberType>().ReverseMap();
            CreateMap<RoleTypeEF, RoleType>().ReverseMap();
            CreateMap<MemberTagEF, MemberTag>().ReverseMap();
            CreateMap<TagEF, Tag>().ReverseMap();
        }       
    }
}
