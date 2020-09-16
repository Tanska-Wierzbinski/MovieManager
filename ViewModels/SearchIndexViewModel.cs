using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.ViewModels
{
    public class SearchIndexViewModel
    {
        public IEnumerable<Actor> Actors { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}
