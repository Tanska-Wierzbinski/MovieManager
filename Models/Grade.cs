using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [Range(1, 10)]
        public int GradeValue { get; set; }

        public int MovieId { get; set; }
      
        public int ActorId { get; set; }

        public Actor Actor { get; set; }
    }
}
