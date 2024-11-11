using System;
using MoviesAPi.Entities;

namespace MoviesAPi.Repository;

public interface IGenresRepository
{
    Task<int> Create(Genre genre);
    Task<List<Genre>> GetAll();
    Task<Genre?> GetById(int id);
    Task<bool> Exists(int id);
    Task Update(Genre genre);
    Task Delete(int id);
}
