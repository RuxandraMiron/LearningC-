namespace ContosoPizza.WebApi.DTOs;

using ContosoPizza.WebApi.Models;


public class PizzaCreateDto
{
    public string Name { get; set; }
    
    public Size Size { get; set; }
    
    public float Price { get; set; }
    
    public List<Ingredient> Ingredients { get; set;}
}