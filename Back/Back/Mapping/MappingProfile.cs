using AutoMapper;
using Back.DTO;
using Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Person, PeopleDTO>().ReverseMap();
			CreateMap<Person, PersonBasicInfoDTO>().ReverseMap();
		}
	}
}
