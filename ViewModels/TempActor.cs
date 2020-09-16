using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class TempActor
    {
        public TempActor()
        {
            Countries = new List<string>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(Countries.Contains(getRegionInfo.DisplayName)))
                {
                    Countries.Add(getRegionInfo.DisplayName);
                }
            }
            Countries.Sort();
        }

        public string Name { get; set; }
       
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        public Gender Gender { get; set; }

        [DisplayName("Birth date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BornDate { get; set; }

        [DisplayName("Death date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeathDate { get; set; }
        
        public string Country { get; set; }

        public List<string> Countries { get; set; }

        [DisplayName("Picture")]
        public Image Image { get; set; }
    }
}
