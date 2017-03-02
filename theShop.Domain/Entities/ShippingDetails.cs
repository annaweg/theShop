using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace theShop.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [Display(Name="Street")]
        public string Street { get; set; }
        [Display(Name = "Zip")]
        public string Zip { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        
    }
}
