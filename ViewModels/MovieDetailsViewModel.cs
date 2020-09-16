using Microsoft.AspNetCore.Mvc.Rendering;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.ViewModels
{
    public class MovieDetailsViewModel : Movie
    {
        public Review Review { get; set; }

        public IEnumerable<SelectListItem> SelectListGrade { get; set; }
    }
}
