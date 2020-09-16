using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MovieManager.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        [DisplayName("Title")]
        public string Name { get; set; }

       
        public string Director { get; set; }

        [DisplayName("Relese date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(4000)]
        public string ImageName { get; set; }

        public IList<Review> Reviews { get; set; }

        [DisplayName("Categories")]
        public IList<MovieCategory> MovieCategories { get; set; }

        [DisplayName("Cast")]
        public IList<MovieActor> MovieActors { get; set; }
    }
}
