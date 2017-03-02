using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace theShop.WebUI.Models
{
    //view model to pass data beween controller and view
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
    }
}