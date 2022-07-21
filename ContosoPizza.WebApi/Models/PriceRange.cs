namespace ContosoPizza.WebApi.Models;

using System.ComponentModel.DataAnnotations;

public class PriceRange {
    
    [Range(0, int.MaxValue)]
    public float MinValue {get; set;}
    
    [Range(0, int.MaxValue)]
    public float MaxValue {get; set;}
    
}