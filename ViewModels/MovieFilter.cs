using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class MovieFilter
    {
        public string Search { get; set; }

        public int YearMin { get; set; }

        public int YearMax { get; set; }

        public int GradeMin { get; set; }

        public int GradeMax { get; set; }

        public int[] Categories { get; set; }
    }
}
