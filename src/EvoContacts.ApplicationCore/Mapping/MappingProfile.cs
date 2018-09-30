using AutoMapper;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EvoContacts.ApplicationCore.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() : base()
        {
            CreateMap<Entities.Contact, Models.Contact>();
            CreateMap<Models.ContactCreate, Entities.Contact>();
            CreateMap<Entities.User, Models.User>();
            CreateMap<Models.UserCreate, Entities.User>();
        }
    }

}
