using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.ViewModels
{
    public class HomeViewModel
    {
        public IList<Movie> TopMovies { get; set; }

        public IList<Movie> NewMovies { get; set; }

    }
}
