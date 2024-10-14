using MealPlan_Business.Models;

namespace MealPlan_Business.Repositories
{
    public interface IMealPlanRepo
    {
        public IEnumerable<MealPlan> GetAllPlan();
        public void AddPlan(MealPlan mealPlan);
    }
}