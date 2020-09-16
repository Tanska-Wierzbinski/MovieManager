using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Name { get; set; }
        
        public string Director { get; set; }

        [DisplayName("Release date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Poster")]
        public Image Image { get; set; }

        public int[] ActorIds { get; set; }

        [DisplayName("Cast")]
        public List<SelectListItem> Actors { get; set; }

        public int[] CategoryIds { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public IList<TempActor> TempActors { get; set; }

        public MovieViewModel()
        {
            TempActors = new List<TempActor>();
        }
    }
}
