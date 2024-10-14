using MealPlan_Business.Models;

namespace MealPlan_Business.Repositories
{
    public interface IUserRepo
    {
        public IEnumerable<User> GetAllUsers();
        public void AddUser(User);
    }
}