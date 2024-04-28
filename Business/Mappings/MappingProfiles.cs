using AutoMapper;
using Business.Dtos;
using DataAccess.Data;

namespace Business.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CustomerDTO, Customer>();
            CreateMap<PostDTO, Post>();
        }
    }
}
