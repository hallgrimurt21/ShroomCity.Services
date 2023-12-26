namespace ShroomCity.Services.Interfaces;
using ShroomCity.Models;
using ShroomCity.Models.Dtos;

public interface IExternalMushroomService
{
    Task<Envelope<ExternalMushroomDto>?> GetMushrooms(int pageSize, int pageNumber);
    Task<ExternalMushroomDto?> GetMushroomByName(string name);
}
