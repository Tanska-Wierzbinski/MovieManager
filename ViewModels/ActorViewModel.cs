using Microsoft.AspNetCore.Mvc.Rendering;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;


namespace MovieManager.ViewModels
{
    public class ActorViewModel : Actor
    {
        [DisplayName("Photo")]
        public Image Image { get; set; }

        public int Age { get; set; }

        public int[] MovieIds { get; set; }

        public List<SelectListItem> Movies { get; set; }

        public IList<TempMovie> TempMovies { get; set; }

        public List<string> Countries { get; set; }

        public ActorViewModel()
        {
            TempMovies = new List<TempMovie>();
            Countries = new List<string>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(Countries.Contains(getRegionInfo.EnglishName)))
                {
                    Countries.Add(getRegionInfo.EnglishName);
                }
            }
            Countries.Sort();
        }
    }
}
