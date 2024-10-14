using MealPlan_Business.Models;
using MealPlan_Business.Repositories;

namespace MealPlan_Business
{
    public class MealPlanService(IMealPlanRepo _mealPlanRepo, IUserRepo _userRepo)
    {
        public void AddMealPlan(int id, string name, List<Meal> meals, DateTime expirationDate)
        {
            MealPlan mealPlan = new()
            {
                Id = id,
                Name = name,
                Meals = meals,
                ExpirationDate = expirationDate
            };

            _mealPlanRepo.AddPlan(mealPlan);
        }
        public void UpdateMealPlan(int id, string name, List<Meal> meals, DateTime expirationDate)
        {
             MealPlan mealPlan = (MealPlan) _mealPlanRepo.GetAllPlan().Where(mp => mp.Id == id);
             mealPlan.Name = name;
             mealPlan.Meals = meals;
             mealPlan.ExpirationDate = expirationDate;
        }

        public MealPlan GetMealPlanByName(string name) 
        {
           return (MealPlan) _mealPlanRepo.GetAllPlan().Where(mp => mp.Name == name);
        }
        public MealPlan GetMealPlanById(int id) 
        {
           return (MealPlan) _mealPlanRepo.GetAllPlan().Where(mp => mp.Id == id);
        }

        public void AddCredit(User user, double amount)
        {
            
        }

    }
}