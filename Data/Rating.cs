using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeConnoisseur.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int CoffeeId { get; set; }
        [NotMapped]
        public string CoffeeName { get; set; }
        public string Comment { get; set; }
        [Range(1, 5)]
        public double RatingValue { get; set; }
    }
}
