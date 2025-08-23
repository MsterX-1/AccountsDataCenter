using Application.DTOs.AccountDto;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AutoMapper
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, GetAccountDto>();
        }
    }
}
