namespace ShroomCity.Services.Interfaces;
using ShroomCity.Models.Dtos;

public interface ITokenService
{
    string GenerateJwtToken(UserDto user);
    Task<bool> IsTokenBlacklisted(int tokenId);
}
