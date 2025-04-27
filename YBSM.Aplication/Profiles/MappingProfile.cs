using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.ModelDTO;

namespace YBSM.Core.Aplication
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserResponseDto, User>();

           /* CreateMap<MccLyPay, addWarehouseRequestDto>().ReverseMap();
            CreateMap<addWarehouseRequestDto, Warehouse>();*/

            
        }
    }
}
