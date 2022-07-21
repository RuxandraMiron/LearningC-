using ContosoPizza.WebApi.Models;
using ContosoPizza.WebApi.DTOs;

namespace ContosoPizza.WebApi.Interfaces;

public interface IPizzaService {
    
    List<Pizza> GetAll();
    
    Pizza Get(int id);
    
    // List<Pizza> GetByPrice(float price);
    
    // List<Pizza> GetByName(string name);
    
    // List<Pizza> GetByIngredient(string ingredient);
    
    IQueryable<Pizza> GetByQuery(PizzaGetDto pizzaGetDto);
    
    void Add(PizzaCreateDto pizzaCreateDto);
    
    void Delete( int id);
    
    Pizza Update(int Id, PizzaUpdateDto pizza);
    
}