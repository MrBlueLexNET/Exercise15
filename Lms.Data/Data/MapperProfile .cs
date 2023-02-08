using AutoMapper;
using Lms.Core.DTOs;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<CreateTournamentDto, Tournament>().ReverseMap();
            CreateMap<TournamentDto, Tournament>().ReverseMap();
        }
    }
}
