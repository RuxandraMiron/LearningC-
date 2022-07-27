using System.Net;
using ContosoPizza.WebApi.Models;
using ContosoPizza.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.WebApi.DTOs;
using ContosoPizza.WebApi.Exceptions;

namespace ContosoPizza.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase {
    
    private readonly IPizzaService pizzaService;
    
    public PizzaController(IPizzaService pizzaService) 
    {
        this.pizzaService = pizzaService;
    }
    
        /// <summary>
    /// Returns all pizzas.
    /// </summary>
    [HttpGet(Name = "GetAll")]
    public ActionResult<List<Pizza>> GetAll() => pizzaService.GetAll();
    
        /// <summary>
    /// Returns a specific pizza.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}", Name = "GetPizzaById")]
    public ActionResult<Pizza> Get(int id) 
    {
        var pizza = pizzaService.Get(id);
        
        if(pizza == null)
        {
            return NotFound();
        }
        
        return Ok(pizza);
    }
    
    // [HttpGet("price")]
    // public ActionResult<List<Pizza>> GetByPrice([FromQuery]float price) 
    // {   
    //     var pizza = pizzaService.GetByPrice(price);
    //     if(pizza == null)
    //     {
    //         return NotFound();
    //     }
        
    //     return Ok(pizza);
    // }
    
    // [HttpGet("name")]
    // public ActionResult<List<Pizza>> GetByName([FromQuery]string name) 
    // {
    //     var pizza = pizzaService.GetByName(name);
    //     if(pizza == null)
    //     {
    //         return NotFound();
    //     }
        
    //     return Ok(pizza);
    // }
    
    // [HttpGet("ingredient")]
    // public ActionResult<List<Pizza>> GetByIngredient([FromQuery]string ingredient) 
    // {
    //     var pizza = pizzaService.GetByIngredient(ingredient);
    //     if(pizza == null)
    //     {
    //         return NotFound();
    //     }
        
    //     return Ok(pizza);
    // }
    
        /// <summary>
    /// Returns pizzas depending on which pqery parameters you provide.
    /// </summary>
    /// <param name="pizzaGetDto"></param>

    [HttpGet("search",Name = "GetPizzaByQuery")]
    public IActionResult GetByQuery ([FromQuery] PizzaGetDto pizzaGetDto)
    {   
        try
        {
            IQueryable<Pizza> pizza = pizzaService.GetByQuery(pizzaGetDto);
            return Ok(pizza);
            
        }
        catch (InvalidOperationException e)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
        }
    }

        /// <summary>
    /// Creates a pizza.
    /// </summary>
    ///<remarks>
    /// Sample request:
    ///
    ///     POST /Pizza
    ///     {
    ///        "name": "Capriciosa",
    ///        "size" : 2,
    ///        "price" : 10
    ///     }
    ///
    /// </remarks>
    [HttpPost(Name = "PostPizza")]
    public IActionResult Create([FromBody] PizzaCreateDto pizzaCreateDto)
    {   
        pizzaService.Add(pizzaCreateDto);
        return Ok(pizzaCreateDto);
    }
    
        /// <summary>
    /// Updates some fields in a specific pizza.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pizzaUpdateDto"></param>
    /// <response code="404">If the id is null</response>
    [HttpPatch("{id}",Name = "PatchPizza")]
    public IActionResult Update(int id, [FromBody] PizzaUpdateDto pizzaUpdateDto)
    {   
        try
        {
            Pizza pizza= pizzaService.Update(id, pizzaUpdateDto);
            return Ok(pizza);         
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
        /// <summary>
    /// Deletes a specific pizza.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="404">If the id is null</response>

    [HttpDelete("{id}", Name = "DeletePizza")]
    public IActionResult Delete(int id)
    {   
        try
        {
            pizzaService.Delete(id);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
        
        return Ok();         
    }
    

}