using System.Linq.Expressions;
using System.Linq;
using System.IO.Enumeration;
using ContosoPizza.WebApi.Models;
using ContosoPizza.WebApi.Interfaces;
using ContosoPizza.WebApi.DTOs;
using ContosoPizza.WebApi.Exceptions;

namespace ContosoPizza.WebApi.Services;

public class PizzaService : IPizzaService {
    
    private readonly ILogger logger;
    private const string SortById = nameof(PizzaGetDto.Id);
    private const string SortByName = nameof(PizzaGetDto.Name);
    private static readonly Expression<Func<Pizza, object>> defaultKeySelector = pizza => pizza.Id;
    
    List<Pizza> Pizzas { get; }
    
    public PizzaService(ILogger<PizzaService> logger) {
        this.logger = logger;
        
        Pizzas = new List<Pizza> {
            new Pizza { Id = 1, Name = "Classic Italiano", Size = Size.Slice, Price = 1.5F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "basil", grams = 10}}},
            new Pizza { Id = 2, Name = "Veggie", Size = Size.Large, Price = 13.5F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "basil", grams = 10},
                                                            new Ingredient {Name = "eggplant", grams = 10}}},
            new Pizza { Id = 3, Name = "Quatro Formaggi", Size = Size.Medium, Price = 9F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "cheddar", grams = 10},
                                                            new Ingredient {Name = "parmesan", grams = 10}}},
            new Pizza { Id = 4, Name = "Quatro Carni", Size = Size.Large, Price = 13.5F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "ham", grams = 10},
                                                            new Ingredient {Name = "salami", grams = 10},
                                                            new Ingredient {Name = "bacon", grams = 10}}}, 
            new Pizza { Id = 5, Name = "Diavola", Size = Size.Small, Price = 7F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "salami", grams = 10},
                                                            new Ingredient {Name = "jalapeno", grams = 10}}},
            new Pizza { Id = 6, Name = "Bambino", Size = Size.Slice, Price = 1.5F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "ham", grams = 10},
                                                            new Ingredient {Name = "corn", grams = 10}}},
            new Pizza { Id = 7, Name = "Capriciosa", Size = Size.Medium, Price = 9F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "ham", grams = 10},
                                                            new Ingredient {Name = "olives", grams = 10},
                                                            new Ingredient {Name = "mushrooms", grams = 10}}},
            new Pizza { Id = 8, Name = "Mexicana", Size = Size.Medium, Price = 9F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "chicken", grams = 10},
                                                            new Ingredient {Name = "beans", grams = 10},
                                                            new Ingredient {Name = "corn", grams = 10}}}, 
            new Pizza { Id = 9, Name = "Chicken Cheese", Size = Size.Large, Price = 13.5F, 
                        Ingredients = new List<Ingredient> {new Ingredient {Name = "tomatoes", grams = 100},
                                                            new Ingredient {Name = "mozzarella", grams = 100},
                                                            new Ingredient {Name = "chicken", grams = 10},
                                                            new Ingredient {Name = "cheddar", grams = 10},
                                                            new Ingredient {Name = "olives", grams = 10}}},                                                                                                                                                                                                                                                                                                                                             
        };
    }
    
    public  List<Pizza> GetAll() => Pizzas;
    
    public  Pizza Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);
    
    // public List<Pizza> GetByPrice(float price) 
    // {   
    //     var pizza = Pizzas.FindAll(p => p.Price <= price);
    //     logger.LogInformation("A list of pizzas by price: {@Pizza}", pizza);

    //     return pizza;
    // }
    
    // public List<Pizza> GetByName (string name)
    // {   
    //     List<Pizza> pizzaList = new List<Pizza>();
    //     foreach(var pizza in Pizzas) 
    //     {
    //         if (FileSystemName.MatchesSimpleExpression($"{name}*",pizza.Name) || pizza.Name.Equals(name))
    //         {
    //             pizzaList.Add(pizza);
    //             logger.LogInformation("A list of pizzas by name: name: {PizzaName}, price: {PizzaPrice}", pizza.Name, pizza.Price);
    //         }
    //     }

    //     return pizzaList;
    // }
    
    // public List<Pizza> GetByIngredient( string ingredient)
    // {
    //     List<Pizza> pizzaList = new List<Pizza>();
    //     foreach(var pizza in Pizzas) 
    //     {
    //         foreach (var ingred in pizza.Ingredients)
    //         {
    //             if(ingred.Name.Equals(ingredient))
    //             {
    //                 pizzaList.Add(pizza);
    //                 logger.LogInformation("A list of pizzas by ingredient: name: {PizzaName}, price: {PizzaPrice}", pizza.Name, pizza.Price);
                
    //             }
    //         }
    //     }

    //     return pizzaList;
    // }
    
    public IQueryable<Pizza> GetByQuery(PizzaGetDto pizzaGetDto)
    {
        IQueryable<Pizza> pizzaList = Filter(pizzaGetDto);
        pizzaList = Sort(pizzaGetDto,pizzaList);
        pizzaList = Paginate(pizzaGetDto, pizzaList);
        
        return pizzaList;
        
    }
    
    public IQueryable<Pizza> Filter(PizzaGetDto pizzaGetDto)
    {
        IQueryable<Pizza> pizzaList = Pizzas.AsQueryable();
        
        if( logger.IsEnabled(LogLevel.Information))
        {
            // logger.LogInformation("pizzaGetDto: {min} - {max} ",pizzaGetDto.PriceRange.MinValue, pizzaGetDto.PriceRange.MaxValue);
        }
        // logger.LogInformation("Id: {id}", pizzaGetDto.Id);
        // logger.LogInformation("Name: {id}", pizzaGetDto.Name);
        // logger.LogInformation("Price: {id}", pizzaGetDto.Price);
        // logger.LogInformation("PriceMin: {id}", pizzaGetDto.PriceMinValue);
        // logger.LogInformation("PriceMax: {id}", pizzaGetDto.PriceMaxValue);
        // logger.LogInformation("Ingredient: {id}", pizzaGetDto.Ingredient);
        // logger.LogInformation("sortByPriceAsc: {id}", pizzaGetDto.SortByPriceAsc);
        // logger.LogInformation("pageSize: {id}", pizzaGetDto.PageSize);
        // logger.LogInformation("pageIndex : {id}", pizzaGetDto.PageIndex);
        
        if (pizzaGetDto.Id.HasValue)
        {
            pizzaList = pizzaList.Where(p => p.Id == pizzaGetDto.Id);
        }
        
        if (!string.IsNullOrWhiteSpace(pizzaGetDto.Name))
        {
            pizzaList = pizzaList.Where(p => p.Name.Contains(pizzaGetDto.Name));
        }
        
        if ( pizzaGetDto.PriceRange is not null)
        {
            pizzaList = pizzaList.Where(p => p.Price >= pizzaGetDto.PriceRange.MinValue && p.Price <= pizzaGetDto.PriceRange.MaxValue);
        }
        
        if (pizzaGetDto.Ingredient is not null)
        {       
            pizzaList = pizzaList.Where(p => p.Ingredients.Any(i => i.Name.Contains(pizzaGetDto.Ingredient)));
        }
        
        return pizzaList;
    }
    
    public IQueryable<Pizza> Sort( PizzaGetDto pizzaGetDto, IQueryable<Pizza> pizzaList)
    {   
        if( pizzaGetDto.IsSortAsc.HasValue && pizzaGetDto.SortBy is null)
        {
            throw new InvalidOperationException("There is no sort by property");
        }
        
        Expression<Func<Pizza, object>> keySelector = GetSelectorBy(pizzaGetDto.SortBy);
        
        if(pizzaGetDto.IsSortAsc.HasValue && !pizzaGetDto.IsSortAsc.Value)
        {
            pizzaList = pizzaList.OrderByDescending(keySelector);
        }
        else
        {
            pizzaList = pizzaList.OrderBy(keySelector);
        }
        
        return pizzaList;
    }
    
    private static Expression<Func<Pizza,object>> GetSelectorBy(string sortByProperty)
    {
        if (SortByName.Equals(sortByProperty, StringComparison.InvariantCultureIgnoreCase))
            {
                return pizza => pizza.Name;
            }

        if (SortById.Equals(sortByProperty, StringComparison.InvariantCultureIgnoreCase))
            {
                return pizza => pizza.Id;
            }
            
        return defaultKeySelector;
    }
    
    public IQueryable<Pizza> Paginate( PizzaGetDto pizzaGetDto, IQueryable<Pizza> pizzaList)
    {   
        int pageIndex = pizzaGetDto.PageIndex ?? 0;
        int pageSize = pizzaGetDto.PageSize ?? 9;
        
        pizzaList = pizzaList.Skip(pageIndex * pageSize).Take(pageSize);
        
        return pizzaList;
    }
    
    public void Add(PizzaCreateDto pizzaCreateDto) 
    {
        int lastID = Pizzas.Last().Id;
        Pizza pizza = new Pizza() { Id = Interlocked.Increment(ref lastID) , Name = pizzaCreateDto.Name , Size = pizzaCreateDto.Size , 
                                    Price = pizzaCreateDto.Price , Ingredients = pizzaCreateDto.Ingredients };
            
        Pizzas.Add(pizza);
        
        logger.LogInformation("A new pizza has been created: name: {PizzaName}, price: {PizzaPrice}", pizza.Name, pizza.Price);
    }
    
    public void Delete( int id) 
    {
        Pizza pizza = Get(id);
        
        if (pizza is null)
        {
            throw new PizzaIdNotFoundException(id);
        }
        else
        {
            Pizzas.Remove(pizza);
        }
    }
    
    public Pizza Update(int id, PizzaUpdateDto pizzaUpdateDto) 
    {
        Pizza pizza = Get(id);
        
        if (pizza is null)
        {
            throw new PizzaIdNotFoundException(id);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(pizzaUpdateDto.Name))
            {
                pizza.Name = pizzaUpdateDto.Name;
            }
            
            if (pizzaUpdateDto.Price.HasValue)
            {
                pizza.Price = (float)pizzaUpdateDto.Price;
            }
        }
        return pizza;
    }
    
    // private static Pizza MapFrom( PizzaGetDto pizzaGetDto)
    // {
    //     Pizza result = new Pizza { Id = (int)pizzaGetDto.Id , Name = pizzaGetDto.Name , Size =  }
    // }
}