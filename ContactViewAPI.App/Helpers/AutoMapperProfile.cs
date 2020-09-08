﻿namespace ContactViewAPI.App.Helpers
{
    using AutoMapper;
    using ContactViewAPI.App.Dtos.Auth;
    using ContactViewAPI.App.Dtos.Contact;
    using ContactViewAPI.Data.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForRegisterDto>();
            CreateMap<User, UserForLoginDto>();

            CreateMap<UserForRegisterDto, User>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(ur => ur.Email))
                .ForMember(m => m.FirstName, opt => opt.MapFrom(ur => ur.Name));
            CreateMap<UserForLoginDto, User>();

            CreateMap<Contact, ContactCreateDto>();
            CreateMap<ContactCreateDto, Contact>();

            CreateMap<Contact, ContactToReturnDto>();
            CreateMap<ContactToReturnDto, Contact>();

        }
    }
}
