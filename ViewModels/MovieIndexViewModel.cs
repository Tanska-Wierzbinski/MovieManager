using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.ViewModels
{
    public class MovieIndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }

        public PaginatedList<Movie> PaginatedMovies { get; set; }

        public MovieFilter Filter { get; set; }

        public IList<Category> Categories { get; set; }
    
        public MovieIndexViewModel()
        {
            Filter = new MovieFilter();
        }
    }
}
