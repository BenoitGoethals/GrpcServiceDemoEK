using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrpcServiceUsers.Protos;

namespace GrpcServiceUsers.mapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, model.User>().ReverseMap();
            //CreateMap<model.User, model.User>().ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid));
            //CreateMap<model.User, model.User>().ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active));
            //CreateMap<model.User, model.User>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            //CreateMap<model.User, model.User>().ForMember(dest => dest.ForName, opt => opt.MapFrom(src => src.ForName));
            //CreateMap<model.User, model.User>().ForMember(dest => dest.LatName, opt => opt.MapFrom(src => src.LatName));


        }
    }
}
