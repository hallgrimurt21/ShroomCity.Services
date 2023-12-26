namespace ShroomCity.Services.Interfaces;
using ShroomCity.Models.Dtos;
using ShroomCity.Models.InputModels;

public interface IAccountService
{
    Task<UserDto?> Register(RegisterInputModel inputModel);
    Task<UserDto?> SignIn(LoginInputModel inputModel);
    Task SignOut(int tokenId);
}
