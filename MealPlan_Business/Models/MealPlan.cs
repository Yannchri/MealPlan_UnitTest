namespace MealPlan_Business.Models;

public class MealPlan
{
    public int Id {get; set;}
    public string Name {get; set;}
    public decimal Price {get; set;}
    public DateTime startDate {get; set;}
    public DateTime endDate {get; set;}
}