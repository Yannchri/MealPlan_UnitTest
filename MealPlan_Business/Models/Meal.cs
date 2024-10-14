namespace MealPlan_Business.Models
{
    public class Meal
    {
        public required String Name;
        public required IEnumerable<String> Ingredients;
        public required String Type;
    }
}