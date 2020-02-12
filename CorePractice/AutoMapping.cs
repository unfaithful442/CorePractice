using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CorePractice.Models;
using CorePractice.ViewModels;

namespace CorePractice
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //config viewmodel to model
            CreateMap<BackEndCreateUser, User>().ForMember(dest => dest.Password, src => src.Ignore()).ForMember(dest => dest.Salt, src => src.Ignore());

            CreateMap<BackEndUpdateGroup, Group>();

            CreateMap<BackEndCreateGroup, Group>();
        }
    }
}
