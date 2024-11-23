namespace MealPlan_Business.Models;

public class MealTransaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}