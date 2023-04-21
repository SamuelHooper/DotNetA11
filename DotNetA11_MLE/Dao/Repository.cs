using DotNetA11_MLE.Context;
using DotNetA11_MLE.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetA11_MLE.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public List<Genre> GetAllGenres()
        {
            return _context.Genres.ToList();
        }

        public List<Movie> GetAllMovies()
        {
            return _context.Movies.ToList();
        }

        public List<Movie> SearchMovies(string searchString)
        {
            var allMovies = GetAllMovies();
            var searchMovies = allMovies.Where(m => m.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return searchMovies.ToList();
        }
    }
}
