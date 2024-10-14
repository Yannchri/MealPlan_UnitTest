
namespace MealPlan_Business.Models
{
    public class MealPlan
    {
        public string Name;
        public required IEnumerable<Meal> Meals { get; set; }
        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}