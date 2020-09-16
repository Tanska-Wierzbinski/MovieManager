using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public enum Gender
    {
        [Display(Name = "Man")]
        Male = 2,
        [Display(Name = "Woman")]
        Female = 0
    }

    public class Actor
    {
        public int ActorId { get; set; }
        
        public string Name { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Born date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BornDate { get; set; }

        [DisplayName("Death date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeathDate { get; set; }
       
        public Gender Gender { get; set; }
      
        public string Country { get; set; }

        public IList<MovieActor> MovieActors { get; set; }

        public IList<Grade> Grades { get; set; }

        public string ImageName { get; set; }
    }
}
