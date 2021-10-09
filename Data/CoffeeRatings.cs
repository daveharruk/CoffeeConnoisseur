using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeConnoisseur.Models
{
    public class CoffeeRatings
    {
        public Coffee RatedCoffee { get; set; }
        public List<Rating> Ratings { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
    }
}
