using System;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MoviesAPi.DTOs;
using MoviesAPi.Entities;
using MoviesAPi.Repository;

namespace MoviesAPi.EndPoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
    {
        // Get List
        group.MapGet("/", GetGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(12)).Tag("genres-get"));
        // Get Single
        group.MapGet("/{id:int}", GetById);
        // Create  
        group.MapPost("/", Create);
        // Update
        group.MapPut("/{id:int}", Update);
        // Delete
        group.MapDelete("/{id:int}", Delete);
        return group;
    }

    // Get List
    static async Task<Ok<List<GenreDTO>>> GetGenres(IGenresRepository repository, IMapper mapper)
    {
        var genres = await repository.GetAll();
        var genresDTOs = mapper.Map<List<GenreDTO>>(genres);
            
        return TypedResults.Ok(genresDTOs);
    }

    // Get Single
    static async Task<Results<Ok<GenreDTO>, NotFound>> GetById(int id, IGenresRepository repository, IMapper mapper)
    {
        var genre = await repository.GetById(id);

        if (genre == null)
        {
            return TypedResults.NotFound();
        }

        var genreDTO = mapper.Map<GenreDTO>(genre);

        return TypedResults.Ok(genreDTO);
    }

    // Create
    static async Task<Created<GenreDTO>> Create(CreateGenreDTO createGenreDTO,
        IGenresRepository repository, 
        IOutputCacheStore outputCacheStore, IMapper mapper)
    {
        var genre = mapper.Map<Genre>(createGenreDTO);

        var id = await repository.Create(genre);
        await outputCacheStore.EvictByTagAsync("genres-get", default);

        var genreDTO = mapper.Map<GenreDTO>(genre);

        return TypedResults.Created($"/genres/{id}", genreDTO);
    }

    // Edit
    static async Task<Results<NotFound, NoContent>> Update(int id, CreateGenreDTO createGenreDTO,
        IGenresRepository repository,
        IOutputCacheStore outputCacheStore, IMapper mapper)
    {
        var exists = await repository.Exists(id); 

        if (!exists)
        {
            return TypedResults.NotFound(); 
        }

        var genre = mapper.Map<Genre>(createGenreDTO);
        genre.Id = id;

        await repository.Update(genre);  
        await outputCacheStore.EvictByTagAsync("genres-get", default); 

        return TypedResults.NoContent();  
    }

    // Delete
    static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository,
        IOutputCacheStore outputCacheStore)
    {
        var exists = await repository.Exists(id);  

        if (!exists)
        {
            return TypedResults.NotFound();  
        }

        await repository.Delete(id);  
        await outputCacheStore.EvictByTagAsync("genres-get", default);  

        return TypedResults.NoContent();  
    }
}
