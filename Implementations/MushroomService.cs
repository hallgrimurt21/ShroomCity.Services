namespace ShroomCity.Services.Implementations;

using ShroomCity.Models;
using ShroomCity.Models.Constants;
using ShroomCity.Models.Dtos;
using ShroomCity.Models.Enums;
using ShroomCity.Models.InputModels;
using ShroomCity.Repositories.Interfaces;
using ShroomCity.Services.Interfaces;
using ShroomCity.Utilities.Exceptions;

public class MushroomService : IMushroomService
{
    private readonly IMushroomRepository mushroomRepository;
    private readonly IExternalMushroomService externalMushroomService;

    public MushroomService(IMushroomRepository mushroomRepository, IExternalMushroomService externalMushroomService)
    {
        this.mushroomRepository = mushroomRepository;
        this.externalMushroomService = externalMushroomService;
    }
    public async Task<int> CreateMushroom(string researcherEmailAddress, MushroomInputModel inputModel)
    {
        var externalMushroom = await this.externalMushroomService.GetMushroomByName(inputModel.Name) ?? throw new MushroomNotFoundException(inputModel.Name);

        var attributes = CreateAttributes(externalMushroom, researcherEmailAddress);

        var mushroomId = await this.mushroomRepository.CreateMushroom(inputModel, researcherEmailAddress, attributes);

        return mushroomId;
    }

    private static List<AttributeDto> CreateAttributes(ExternalMushroomDto externalMushroom, string researcherEmailAddress)
    {
        var attributes = new List<AttributeDto>();

        if (externalMushroom != null)
        {
            foreach (var color in externalMushroom.Colors)
            {
                attributes.Add(new AttributeDto
                {
                    Value = color,
                    Type = AttributeTypeConstants.Color,
                    RegisteredBy = researcherEmailAddress,
                });
            }
            foreach (var shape in externalMushroom.Shapes)
            {
                attributes.Add(new AttributeDto
                {
                    Value = shape,
                    Type = AttributeTypeConstants.Shape,
                    RegisteredBy = researcherEmailAddress,
                });
            }
            foreach (var surface in externalMushroom.Surfaces)
            {
                attributes.Add(new AttributeDto
                {
                    Value = surface,
                    Type = AttributeTypeConstants.Surface,
                    RegisteredBy = researcherEmailAddress,
                });
            }
        }

        return attributes;
    }

    public Task<bool> CreateResearchEntry(int mushroomId, string researcherEmailAddress, ResearchEntryInputModel inputModel) => this.mushroomRepository.CreateResearchEntry(mushroomId, researcherEmailAddress, inputModel);

    public Task<bool> DeleteMushroomById(int mushroomId) => this.mushroomRepository.DeleteMushroomById(mushroomId);

    public async Task<Envelope<MushroomDto>?> GetLookupMushrooms(int pageSize, int pageNumber)
    {
        var envelope = await this.externalMushroomService.GetMushrooms(pageSize, pageNumber);
        if (envelope!.Items == null)
        {
            throw new MushroomDataMissingException();
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

    public async Task<Envelope<MushroomDto>?> GetMushrooms(string? name, int? stemSizeMinimum, int? stemSizeMaximum, int? capSizeMinimum, int? capSizeMaximum, string? color, int pageSize, int pageNumber)
    {
        var (totalPages, mushrooms) = await this.mushroomRepository.GetMushroomsByCriteria(name, stemSizeMinimum, stemSizeMaximum, capSizeMinimum, capSizeMaximum, color, pageSize, pageNumber);
        return new Envelope<MushroomDto>
        {
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalPages = totalPages,
            Items = mushrooms.ToList()
        };
    }

    public async Task<bool> UpdateMushroomById(int mushroomId, MushroomUpdateInputModel inputModel, bool performLookup)
    {
        if (performLookup)
        {
            var mushroom = await this.mushroomRepository.GetMushroomById(mushroomId);
            if (mushroom == null)
            {
                return false;
            }
            var externalMushroom = await this.externalMushroomService.GetMushroomByName(mushroom.Name);
            if (externalMushroom == null)
            {
                return false;
            }
            var updatedMushroom = new MushroomUpdateInputModel { Name = externalMushroom.Name, Description = externalMushroom.Description };
            var isSuccessful = await this.mushroomRepository.UpdateMushroomById(mushroomId, updatedMushroom);
            return isSuccessful;
        }
        else
        {
            return await this.mushroomRepository.UpdateMushroomById(mushroomId, inputModel);
        }
    }
}
