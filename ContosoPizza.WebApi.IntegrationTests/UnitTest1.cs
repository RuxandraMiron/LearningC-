using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using ContosoPizza.WebApi.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Net;
namespace IntegrationTest;

[TestClass]
public class IntegrationTests
{
    private HttpClient _httpClient;
   
    public IntegrationTests()
    {   
        var webAppFactory = new WebApplicationFactory<ContosoPizza.WebApi.Startup>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }
    
    [TestMethod] 
    public async Task DefaultRoute_ReturnsHelloWorld()
    {
      var response = await _httpClient.GetAsync("");
      var stringResult = await response.Content.ReadAsStringAsync();
      Assert.AreEqual("Hello World!", stringResult);
    }
    
     [TestMethod]
    public async Task GetByQuery_WithNoFilter_ReturnsEntireList() 
    {
      var response = await _httpClient.GetAsync("/pizza/search");
      List<Pizza> Pizzas = new List<Pizza>();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
      response.Content.Should().Be(Pizzas);
    }
    
    
}