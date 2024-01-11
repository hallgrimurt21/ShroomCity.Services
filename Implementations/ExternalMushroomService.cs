namespace ShroomCity.Services.Implementations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ShroomCity.Models;
using ShroomCity.Models.Dtos;
using ShroomCity.Services.Interfaces;

public class ExternalMushroomService : IExternalMushroomService
{
    private readonly HttpClient httpClient;

    public ExternalMushroomService(HttpClient httpClient) => this.httpClient = httpClient;
    public async Task<ExternalMushroomDto?> GetMushroomByName(string name)
    {
        var response = await this.httpClient.GetAsync($"https://mushrooms-api-a309dd19945c.herokuapp.com/mushrooms/{name}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalMushroomDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        }

        return null;
    }

    public async Task<Envelope<ExternalMushroomDto>?> GetMushrooms(int pageSize, int pageNumber)
    {
        var response = await this.httpClient.GetAsync($"https://mushrooms-api-a309dd19945c.herokuapp.com/mushrooms/?pageNumber={pageNumber}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Envelope<ExternalMushroomDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return null;
    }
}
