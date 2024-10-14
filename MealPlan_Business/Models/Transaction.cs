using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business.Models
{
    public class Transaction
    {
        public User User { get; set; }

        public int Id { get; set; }

        public double Amount { get; set; }

        public DateOnly Date { get; set; }

    }
}
