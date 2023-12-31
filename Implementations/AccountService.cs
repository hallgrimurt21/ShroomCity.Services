namespace ShroomCity.Services.Implementations;
using ShroomCity.Models.Dtos;
using ShroomCity.Models.InputModels;
using ShroomCity.Repositories.Interfaces;
using ShroomCity.Services.Interfaces;

public class AccountService : IAccountService
{
    private readonly IAccountRepository accountRepository;
    private readonly ITokenRepository tokenRepository;

    public AccountService(IAccountRepository accountRepository, ITokenRepository tokenRepository)
    {
        this.accountRepository = accountRepository;
        this.tokenRepository = tokenRepository;
    }

    public async Task<UserDto?> Register(RegisterInputModel inputModel) => await this.accountRepository.Register(inputModel);

    public async Task<UserDto?> SignIn(LoginInputModel inputModel) => await this.accountRepository.SignIn(inputModel);

    public async Task SignOut(int tokenId) => await this.tokenRepository.BlacklistToken(tokenId);
}
