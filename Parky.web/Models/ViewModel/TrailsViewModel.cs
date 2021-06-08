using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Parky.web.Models.ViewModel
{
    public class TrailsViewModel
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }
    }
}
