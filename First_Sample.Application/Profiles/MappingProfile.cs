﻿using AutoMapper;
using First_Sample.Application.Dtos.User;
using First_Sample.Domain.Entities;
using First_Sample.Shared.Dtos.Answer;
using First_Sample.Shared.ViewModels.Answers;
using First_Sample.Shared.ViewModels.Users;

namespace First_Sample.Application.Mapping_Profile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto,UserVM>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AnswerDto, AnswerVM>().ReverseMap();
            CreateMap<Answer, AnswerDto>().ReverseMap();
        }
    }
}
