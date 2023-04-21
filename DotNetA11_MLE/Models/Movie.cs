namespace DotNetA11_MLE.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }


        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }

        public Movie(string title, DateTime releaseDate)
        {
            Title = title;
            ReleaseDate = releaseDate;
            MovieGenres = new List<MovieGenre>();
            UserMovies = new List<UserMovie>();
        }

        public string GetFormattedGenres()
        {
            var movieGenres = MovieGenres.ToList();
            string genres = "";

            for (int i = 0; i < movieGenres.Count; i++)
            {
                // Add proper punctuation and grammer
                switch (movieGenres.Count)
                {
                    case 1: genres += $"{movieGenres[0].Genre.Name}"; break;
                    case 2: genres += $"{movieGenres[0].Genre.Name} and {movieGenres[1].Genre.Name}"; i++; break;
                    default:
                        if (i == movieGenres.Count - 1)
                        {
                            genres += $"and {movieGenres[i].Genre.Name}";
                        }
                        else
                        {
                            genres += $"{movieGenres[i].Genre.Name}, ";
                        }
                        break;
                }
            }

            return genres;
        }
    }
}
