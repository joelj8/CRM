using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models.ProfileConfig
{
    public class CustomerProfile : Profile
    {

        public MapperConfiguration GetProfile() {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerApi, Customer>()
                    .ForMember(dest => dest.ID, act => act.MapFrom(src => src.Codigo))
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Nombre))
                    .ForMember(dest => dest.Mail, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Direccion))
                    .ForMember(dest => dest.fechaNacimiento, act => act.MapFrom(src => src.fechaNac != null ? DateTime.Parse(src.fechaNac) : new DateTime()))
                    .ForMember(dest => dest.GenderId, act => act.MapFrom(src => src.Genero))
                    .ForMember(dest => dest.Phone, act => act.MapFrom(src => src.Telefono)).ReverseMap();
            });
            return config;
        }
        
    }
}
