namespace ContosoPizza.WebApi.Models;

using System.ComponentModel.DataAnnotations;

public class Ingredient {
    
    [Required]
    public string Name {get; set;}
    
    public float grams {get; set;}
}