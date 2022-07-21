namespace ContosoPizza.WebApi.Exceptions;

public class PizzaIdNotFoundException : Exception
{
    public PizzaIdNotFoundException(string message)
        : base(message)
    {
    }

    public PizzaIdNotFoundException(int id) 
        : base($"You provided an invalid id : There is no pizza with id {id}")
    {
        Id = id;
    }
    
    public int Id { get; set;}
}