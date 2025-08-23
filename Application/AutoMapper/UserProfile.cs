using Application.DTOs.ItemDto;
using Application.DTOs.UserDto;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AutoMapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User,GetUserDto>();
            CreateMap<User, GetUserWithAccountsDto>()
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts));
            CreateMap<CreateUserDto,User>();
            CreateMap<UpdateUserDto,User>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));// only map if source member is not null
        }
    }
}
