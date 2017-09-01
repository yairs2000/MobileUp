using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileUp.Models
{
    public class Listings
    {
        [Key]
        public int ListingId { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}
