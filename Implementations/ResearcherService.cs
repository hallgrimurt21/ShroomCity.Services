namespace ShroomCity.Services.Implementations;
using ShroomCity.Models.Dtos;
using ShroomCity.Models.InputModels;
using ShroomCity.Repositories.Interfaces;
using ShroomCity.Services.Interfaces;

public class ResearcherService : IResearcherService
{
    private readonly IResearcherRepository researcherRepository;

    public ResearcherService(IResearcherRepository researcherRepository)
        => this.researcherRepository = researcherRepository;
    public Task<int?> CreateResearcher(string createdBy, ResearcherInputModel inputModel)
        => this.researcherRepository.CreateResearcher(createdBy, inputModel);

    public Task<IEnumerable<ResearcherDto>?> GetAllResearchers()
        => this.researcherRepository.GetAllResearchers();

    public Task<ResearcherDto?> GetResearcherByEmailAddress(string emailAddress)
        => this.researcherRepository.GetResearcherByEmailAddress(emailAddress);

    public Task<ResearcherDto?> GetResearcherById(int id)
        => this.researcherRepository.GetResearcherById(id);
}
