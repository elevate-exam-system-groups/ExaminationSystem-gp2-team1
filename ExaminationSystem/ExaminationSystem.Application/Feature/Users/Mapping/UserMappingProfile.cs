using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Application.Feature.Users.Dto;
namespace ExaminationSystem.Application.Feature.Users.Mapping
{
    public class UserMappingProfile :Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Entities.User, UserResponseDto>();
        }
    }
}
