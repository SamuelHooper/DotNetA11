using DotNetA11_MLE.Dao;
using DotNetA11_MLE.Models;
using Microsoft.IdentityModel.Tokens;
using Trivial.CommandLine;

namespace DotNetA11.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository _repository;

        public MovieService(IRepository repository)
        {
            _repository = repository;
        }

        // Main Program code goes here
        public void Invoke()
        {
            string menuOption;

            do
            {
                Console.WriteLine("\n1) Add new movie");
                Console.WriteLine("2) Edit movie");
                Console.WriteLine("3) Display movies");
                Console.WriteLine("4) Search movies");
                Console.WriteLine("5) Delete movie");
                Console.WriteLine("Enter any other key to exit\n");
                menuOption = Console.ReadLine();
                if (menuOption == "1")
                {
                    AddMovie();
                }
                else if (menuOption == "2")
                {
                    EditMovie();
                }
                else if (menuOption == "3")
                {
                    DisplayMovies();
                }
                else if (menuOption == "4")
                {
                    Console.WriteLine("\nWhat movie are you looking for?");
                    DisplaySearchResults(Console.ReadLine());
                }
                else if (menuOption == "5")
                {
                    DeleteMovie();
                }
            } while (menuOption == "1" || menuOption == "2" || menuOption == "3" || menuOption == "4" || menuOption == "5");
        }

        private void AddMovie()
        {
            Movie newMovie;

            Console.Write("Enter a movie title: ");
            string title = ValidateInput();

            Console.WriteLine($"When is the release date of {title}?");
            DateTime releaseDate = ValidateDateTime();

            newMovie = new Movie(title, releaseDate);

            AddGenres(newMovie);

            _repository.AddMovie(newMovie);
        }

        private void AddGenres(Movie movie)
        {
            Console.WriteLine("Loading...");
            var genreCol = new Trivial.Collection.SelectionData<string>();
            var genres = _repository.GetAllGenres();

            foreach (var genre in genres)
            {
                genreCol.Add($"{genre.Name}");
            }
            // Create a options for display.
            var options = new SelectionConsoleOptions
            {
                Question = "Please select a genre: ",
                Tips = "Tips: You can use arrow key to select and press ENTER key to select",
                SelectedPrefix = ">",
                Prefix = "",
                Column = 3,
                MaxRow = 20,
            };

            var genreIds = new List<int>();
            Console.WriteLine($"What genre(s) is {movie.Title}?");
            do
            {
                var genreId = DefaultConsole.Select(genreCol, options).Index;
                if (genreId >= 0 && genreId <= genres.Count - 1)
                {
                    genreIds.Add(genreId);
                }
                Console.Write("Add another genre (Y/n)? ");
            } while (Console.ReadLine().ToLower().First() == 'y');

            foreach (var id in genreIds)
            {
                var newMovieGenre = new MovieGenre
                {
                    Movie = movie,
                    Genre = genres[id]
                };

                movie.MovieGenres.Add(newMovieGenre);
            }
        }

        private void DeleteMovie()
        {
            var selectedMovie = SelectMovie("Which movie would you like to delete?");
            Console.WriteLine($"Are you sure you want to delete {selectedMovie.Title} (Y/n)?");
            if (Console.ReadLine().ToLower().First() == 'y')
            {
                _repository.DeleteMovie(selectedMovie);
            } else
            {
                Console.WriteLine("Canceled");
            }
        }

        private void DisplayMovies()
        {
            Console.WriteLine("How many movies would you like to display?");
            while (true)
            {
                if (!Int32.TryParse(Console.ReadLine(), out int movieCount) || movieCount <= 0)
                {
                    Console.WriteLine("Not a valid amount!");
                }
                else
                {
                    var movieCol = new Trivial.Collection.SelectionData<string>();
                    var movies = _repository.GetAllMovies();
                    Console.WriteLine("Loading...");
                    for (int i = 0; i < movieCount; i++)
                    {
                        movieCol.Add($"{movies[i].Title} | {movies[i].GetFormattedGenres()}");
                    }
                    // Create a options for display.
                    var options = new SelectionConsoleOptions
                    {
                        Tips = "Tips: You can use arrow key to select and press ENTER key to exit",
                        SelectedPrefix = ">",
                        Prefix = "",
                        Column = 2,
                        MaxRow = 20,
                    };
                    DefaultConsole.Select(movieCol, options);
                }
            }
        }

        private void DisplaySearchResults(string searchString)
        {
            var movies = _repository.SearchMovies(searchString);
            Console.WriteLine($"Found {movies.Count} results");
            foreach (var movie in movies)
            {
                Console.WriteLine($"{movie.Title} | {movie.GetFormattedGenres()}");
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private void EditMovie()
        {
            var selectedMovie = SelectMovie("Which movie would you like to edit?");
            Console.WriteLine($"What would you like to update about {selectedMovie.Title}?");
            Console.WriteLine("1) Movie title");
            Console.WriteLine("2) Movie release date");
            Console.WriteLine("3) Movie genres");
            Console.WriteLine("Press any other key to cancel");
            string editOption = Console.ReadLine();
            if (editOption == "1")
            {
                Console.WriteLine("Enter the updated movie name:");
                selectedMovie.Title = ValidateInput();
            }
            else if (editOption == "2")
            {
                Console.WriteLine("Enter the updated movie release date:");
                selectedMovie.ReleaseDate = ValidateDateTime();
            }
            else if (editOption == "3")
            {
                selectedMovie.MovieGenres = new List<MovieGenre>();
                AddGenres(selectedMovie);
            }
            _repository.SaveChanges();
        }

        private Movie SelectMovie(string question)
        {
            var movieCol = new Trivial.Collection.SelectionData<string>();

            Console.WriteLine(question);
            var movies = _repository.SearchMovies(Console.ReadLine());

            Console.WriteLine("Loading...");
            foreach (var movie in movies)
            {
                movieCol.Add($"{movie.Title} | {movie.GetFormattedGenres()}");
            }
            // Create a options for display.
            var options = new SelectionConsoleOptions
            {
                Question = "Please select a movie: ",
                Tips = "Tips: You can use arrow key to select and press ENTER key to select",
                SelectedPrefix = ">",
                Prefix = "",
                Column = 2,
                MaxRow = 20,
            };
            return movies[DefaultConsole.Select(movieCol, options).Index];
        }

        private DateTime ValidateDateTime()
        {
            while (true)
            {
                try
                {
                    return DateTime.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid date!");
                }
            }
        }

        private string ValidateInput()
        {
            string userInput;
            while (true)
            {
                userInput = Console.ReadLine();
                if (!userInput.IsNullOrEmpty())
                {
                    return userInput;
                }
                Console.WriteLine("Cannot be null or empty!");
            }
        }
    }
}