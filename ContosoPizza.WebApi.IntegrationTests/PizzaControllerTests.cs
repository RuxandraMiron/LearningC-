
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using ContosoPizza.WebApi.Models;
using ContosoPizza.WebApi.DTOs;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Net;
using System.Text;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace PizzaControllerTests;

[TestClass]
public class PizzaControllerTests
{
    private HttpClient _httpClient;

    public PizzaControllerTests()
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
    public async Task GetByQuery_WithBeans_ReturnsPizzaMexicana()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/pizza/search?ingredient=beans");
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsNotNull(response.Content.ReadAsStringAsync());
        }

    }

    [TestMethod]
    public async Task GetByQuery_WithInvalidRange_ReturnsValidationError()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/pizza/search?pricerange.minvalue=10&pricerange.maxvalue=3");
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

            Assert.AreEqual("You provided an invalid range for the price: 10, the value for the minimum price is greater than 3, the maximum value", responseBody.Errors[""].GetValue(0));
        }
    }

     [TestMethod]
    public async Task GetByQuery_WithoutSortBy_ReturnsException()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/pizza/search?issortasc=true");
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("There is no sort by property", responseBody);
        }
    }

    [TestMethod]
    public async Task Create_WithCorrectPizza_ReturnsAddedPizza()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/pizza", new PizzaCreateDto { Name = "Chicken Cheese", Size = Size.Large, Price = 13.5F } );

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    [TestMethod]
    public async Task Delete_WithCorrectId_ReturnsDeletedPizza()
    {   
        int id = 2;
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/pizza/{id}");

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

      [TestMethod]
    public async Task Delete_WithInvlidId_ReturnsException()
    {   
        int id = 14;
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/pizza/{id}");

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            string responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual($"You provided an invalid id : There is no pizza with id {id}", responseBody);
        }
    }

    [TestMethod]
    public async Task Update_WithCorrectId_ReturnsUpdatedPizza()
    {
        int id = 2;
        var pizzaUpdate = new PizzaCreateDto { Name = "Chicken Cheese Test", Price = 13.5F };
        var json = JsonConvert.SerializeObject(pizzaUpdate);

        HttpResponseMessage response = await _httpClient.PatchAsync( $"/pizza/{id}", new StringContent(json, Encoding.UTF8, "application/json"));

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

       [TestMethod]
    public async Task Update_WithInvlidId_ReturnsException()
    {   
        int id = 14;
        var pizzaUpdate = new PizzaCreateDto { Name = "Chicken Cheese Test", Price = 13.5F };
        var json = JsonConvert.SerializeObject(pizzaUpdate);

        HttpResponseMessage response = await _httpClient.PatchAsync($"/pizza/{id}", new StringContent(json, Encoding.UTF8, "application/json"));

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            string responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual($"You provided an invalid id : There is no pizza with id {id}", responseBody);
        }
    }
}