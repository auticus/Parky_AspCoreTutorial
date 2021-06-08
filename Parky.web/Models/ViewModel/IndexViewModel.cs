using System.Collections.Generic;

namespace Parky.web.Models.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }
        public IEnumerable<Trail> Trails { get; set; }
    }
}
