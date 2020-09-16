using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.ViewModels
{
    public class ActorIndexViewModel
    {
        public IEnumerable<Actor> Actors { get; set; }

        public PaginatedList<Actor> PaginatedActors { get; set; }

        public ActorFilter Filter { get; set; }

        public List<string> Countries { get; set; }


        public ActorIndexViewModel()
        {
            Filter = new ActorFilter();
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
