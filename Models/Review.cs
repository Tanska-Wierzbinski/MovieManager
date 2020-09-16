using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
      
        public string Description { get; set; }

        [Range(1,10)]
        public int Grade { get; set; }
      
        public string Author { get; set; }
        
        public int MovieId { get; set; }
       
        public Movie Movie { get; set; }
    }
}
