using AutoMapper;
using StaffManagementApi.Models.Dtos;

namespace StaffManagementApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Staff, StaffDto>().ReverseMap();
    }
}