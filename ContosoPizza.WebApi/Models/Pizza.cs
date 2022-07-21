namespace ContosoPizza.WebApi.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
public class Pizza{ 
    
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "Name length can't be more than 50.")]
    public string Name { get; set; }
    
    public Size Size { get; set; }
    
    // [Required]
    // [Range(0, 99.9)]
    public float Price { get; set; }
    [Required]
    public List<Ingredient> Ingredients { get; set;}
    
}

public enum Size {
    
    Slice,
    Small,
    Medium,
    Large
    
}