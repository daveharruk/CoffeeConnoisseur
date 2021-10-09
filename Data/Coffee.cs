using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeConnoisseur.Models
{
    public partial class Coffee
    {
        public Coffee()
        {
        }

        public Coffee(string coffeeName)
        {
            CoffeeName = coffeeName;
        }

        public int CoffeeId { get; set; }

        public string CoffeeName { get; set; }
    }
}
