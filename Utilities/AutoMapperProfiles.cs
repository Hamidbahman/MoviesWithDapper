using System;
using AutoMapper;
using MoviesAPi.DTOs;
using MoviesAPi.Entities;

namespace MoviesAPi.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Genre, GenreDTO>();
        CreateMap<CreateGenreDTO, Genre>();
    }
}
