using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using AlzaApi.App.EndToEndTests.Base;
using AlzaApi.Common.DTOs.Product;

using FluentAssertions;

namespace AlzaApi.App.EndToEndTests;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_Returns200WithList()
    {
        var response = await _client.GetAsync("/api/v1/products");
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK, because: body);

        var products = JsonSerializer.Deserialize<List<ProductListDto>>(body, JsonOptions);
        products.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ExistingId_Returns200WithProduct()
    {
        var id = await GetFirstProductIdAsync();

        var response = await _client.GetAsync($"/api/v1/products/{id}");
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK, because: body);

        var product = JsonSerializer.Deserialize<ProductListDto>(body, JsonOptions);
        product.Should().NotBeNull();
        product!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetById_NonExistingId_Returns404()
    {
        var response = await _client.GetAsync($"/api/v1/products/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task UpdateDescription_ExistingId_Returns204()
    {
        var id = await GetFirstProductIdAsync();

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/products/{id}/description", "Updated description");
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: body);
    }

    [Fact]
    public async Task UpdateDescription_NonExistingId_Returns404()
    {
        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/products/{Guid.NewGuid()}/description", "Updated description");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateDescription_ActuallyPersists()
    {
        var id = await GetFirstProductIdAsync();
        const string newDescription = "Persisted description";

        await _client.PatchAsJsonAsync($"/api/v1/products/{id}/description", newDescription);

        var response = await _client.GetAsync($"/api/v1/products/{id}");
        var body = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<ProductListDto>(body, JsonOptions);

        product!.Description.Should().Be(newDescription);
    }


    private async Task<Guid> GetFirstProductIdAsync()
    {
        var response = await _client.GetAsync("/api/v1/products");
        var body = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, because: body);

        var products = JsonSerializer.Deserialize<List<ProductListDto>>(body, JsonOptions);
        products.Should().NotBeNullOrEmpty("seed data must exist");

        return products!.First().Id;
    }
}
