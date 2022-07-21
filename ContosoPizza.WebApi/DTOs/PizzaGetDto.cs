namespace ContosoPizza.WebApi.DTOs;

using System.ComponentModel.DataAnnotations;
using ContosoPizza.WebApi.Models;


public class PizzaGetDto : IValidatableObject
{   
    
    [Range(1, int.MaxValue)]
    public int? Id { get; set; }
    
    [MinLength(2)]
    public string Name { get; set; }
    
    public PriceRange PriceRange { get; set;}
    
    public string Ingredient { get; set;}
    
    public bool? IsSortAsc { get; set;}
    
    public string SortBy { get; set; }
    
    [Range(1, 1000)]
    public int? PageSize { get; set; }

    [Range(0, int.MaxValue)]
    public int? PageIndex { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PriceRange is not null)
        {
            if ( PriceRange.MaxValue < PriceRange.MinValue)
            {
                yield return new ValidationResult( $"You provided an invalid range for the price: {PriceRange.MinValue}, the value for the minimum price is greater than {PriceRange.MaxValue}, the maximum value");
            }
        }
        
    }
}