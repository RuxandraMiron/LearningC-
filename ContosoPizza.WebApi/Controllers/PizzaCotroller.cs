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
    
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() => pizzaService.GetAll();
    
    [HttpGet("{id}")]
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
    
    [HttpGet("search")]
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

    [HttpPost]
    public IActionResult Create([FromBody] PizzaCreateDto pizzaCreateDto)
    {   
        pizzaService.Add(pizzaCreateDto);
        return Ok(pizzaCreateDto);
    }
    
    [HttpPatch("{id}")]
    public IActionResult Update(int Id, [FromBody] PizzaUpdateDto pizzaUpdateDto)
    {   
        try
        {
            Pizza pizza= pizzaService.Update(Id, pizzaUpdateDto);
            return Ok(pizza);         
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
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