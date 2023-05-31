using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Mapper
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<Competition, CompetitonCreateDto>().ReverseMap();
        }
    }
}
