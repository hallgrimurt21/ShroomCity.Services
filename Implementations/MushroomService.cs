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

    public Task<bool> CreateResearchEntry(int mushroomId, string researcherEmailAddress, ResearchEntryInputModel inputModel) => this.mushroomRepository.CreateResearchEntry(mushroomId, researcherEmailAddress, inputModel);

    public Task<bool> DeleteMushroomById(int mushroomId) => this.mushroomRepository.DeleteMushroomById(mushroomId);

    public async Task<Envelope<MushroomDto>?> GetLookupMushrooms(int pageSize, int pageNumber)
    {
        var envelope = await this.externalMushroomService.GetMushrooms(pageSize, pageNumber);
        if (envelope == null)
        {
            return null;
        }

        var mushrooms = envelope.Items.Select(m => new MushroomDto
        {
            Id = int.TryParse(m.Id, out var id) ? id : null,
            Name = m.Name,
            Description = m.Description
        }).ToList();

        return new Envelope<MushroomDto>
        {
            PageSize = envelope.PageSize,
            PageNumber = envelope.PageNumber,
            TotalPages = envelope.TotalPages,
            Items = mushrooms
        };
    }

    public Task<MushroomDetailsDto?> GetMushroomById(int id) => this.mushroomRepository.GetMushroomById(id);

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
