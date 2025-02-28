using System;
using AutoMapper;
using Wörterbuch.AppData.Entities;
using Wörterbuch.ViewModels;

namespace Wörterbuch.AppLib;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<WordEntity, WordEntityVM>().ReverseMap();
	}
}
