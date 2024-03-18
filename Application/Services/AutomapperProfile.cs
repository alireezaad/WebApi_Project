using Application.Models.TaskEntityModels;
using Application.Models.UserModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Application.Services
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<TaskEntityPostModel, TaskEntity>();
            CreateMap<TaskEntity, TaskEntityGetModel>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(t => t.Id)));

            CreateMap<UserPostModel, User>();
            CreateMap<User, UserGetModel>()
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.Select(u => u.Id)));

            CreateMap<UserPutModel, User>()
                .ForMember(dest => dest.Tasks, opt => opt.Ignore());

            CreateMap<User, UserGetModel>()
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.Select(t => t.Id)));
        }
    }
}
