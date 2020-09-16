using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class TempMovie
    {
        [DisplayName("Title")]
        public string Name { get; set; }

        public string Director { get; set; }

        [DisplayName("Release date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        public int[] CategoryIds { get; set; }

        public List<SelectListItem> Categories { get; set; }
    }
}
