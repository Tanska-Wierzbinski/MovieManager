using Microsoft.AspNetCore.Http;
using MovieManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class Image
    {
        public int ImageId { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
