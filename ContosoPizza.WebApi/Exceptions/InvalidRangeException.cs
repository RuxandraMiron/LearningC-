namespace ContosoPizza.WebApi.Exceptions;

public class InvalidRangeException : Exception
{
    public InvalidRangeException(string message)
        : base(message)
    {
    }

    public InvalidRangeException(float? min, float? max) 
        : base($"You provided an invalid range for the price: {min}, the value for the minimum price is greater than {max}, the maximum value")
    {
        Min = min;
        Max = max;
    }
    
    public float? Min {get; set;}
    public float? Max {get; set;}
}