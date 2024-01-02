namespace ShroomCity.Services.Implementations;
using ShroomCity.Models;
using ShroomCity.Models.Dtos;
using ShroomCity.Models.InputModels;
using ShroomCity.Repositories.Interfaces;
using ShroomCity.Services.Interfaces;

public class MushroomService : IMushroomService
{
    private readonly IMushroomRepository mushroomRepository;
    private readonly IExternalMushroomService externalMushroomService;

    public MushroomService(IMushroomRepository mushroomRepository, IExternalMushroomService externalMushroomService)
    {
        this.mushroomRepository = mushroomRepository;
        this.externalMushroomService = externalMushroomService;
    }
    public Task<int> CreateMushroom(string researcherEmailAddress, MushroomInputModel inputModel)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateResearchEntry(int mushroomId, string researcherEmailAddress, ResearchEntryInputModel inputModel)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMushroomById(int mushroomId)
    {
        throw new NotImplementedException();
    }

    public Task<Envelope<MushroomDto>?> GetLookupMushrooms(int pageSize, int pageNumber)
    {
        throw new NotImplementedException();
    }

    public Task<MushroomDetailsDto?> GetMushroomById(int id)
    {
        throw new NotImplementedException();
    }

    public Envelope<MushroomDto>? GetMushrooms(string? name, int? stemSizeMinimum, int? stemSizeMaximum, int? capSizeMinimum, int? capSizeMaximum, string? color, int pageSize, int pageNumber)
    {
        var (totalPages, mushrooms) = this.mushroomRepository.GetMushroomsByCriteria(name, stemSizeMinimum, stemSizeMaximum, capSizeMinimum, capSizeMaximum, color, pageSize, pageNumber);
        return new Envelope<MushroomDto>
        {
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalPages = totalPages,
            Items = mushrooms.ToList()
        };
    }

    public Task<bool> UpdateMushroomById(int mushroomId, MushroomUpdateInputModel inputModel, bool performLookup)
    {
        throw new NotImplementedException();
    }
}
