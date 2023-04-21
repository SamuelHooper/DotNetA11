using DotNetA11_MLE.Models;

namespace DotNetA11_MLE.Dao
{
    public interface IRepository
    {
        void AddMovie(Movie movie);
        void DeleteMovie(Movie movie);
        void SaveChanges();
        List<Genre> GetAllGenres();
        List<Movie> GetAllMovies();
        List<Movie> SearchMovies(string searchString);
    }
}
