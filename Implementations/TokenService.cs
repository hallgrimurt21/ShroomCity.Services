namespace ShroomCity.Services.Implementations;
using Microsoft.IdentityModel.Tokens;
using ShroomCity.Models.Constants;
using ShroomCity.Models.Dtos;
using ShroomCity.Repositories.DbContext;
using ShroomCity.Services.Interfaces;
using ShroomCity.Utilities.Exceptions;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtConfiguration
{
    /// <summary>
    /// The secret used to sign the JWT token
    /// </summary>
    public string Secret { get; set; } = "";
    /// <summary>
    /// Expiration in minutes for the JWT token.
    /// </summary>
    public string ExpirationInMinutes { get; set; } = "";
    /// <summary>
    /// The issuer of the JWT token. If the issuer is not a known enity, the JWT token should be rejected. In our
    /// example this API is the issuer - but that is not always the case.
    /// </summary>
    public string Issuer { get; set; } = "";
    /// <summary>
    /// The audience of the token. The services which are expected to receive and use the token. In our example
    /// this API is the audience - but that is not always the case.
    /// </summary>
    public string Audience { get; set; } = "";
}

public class TokenService : ITokenService
{
    private readonly JwtConfiguration jwtConfig;
    private readonly ShroomCityDbContext context;

    public TokenService(ShroomCityDbContext context, JwtConfiguration jwtConfig)
    {
        this.context = context;
        this.jwtConfig = jwtConfig;
    }

    public string GenerateJwtToken(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.EmailAddress),
            // Add additional claims as needed
        };

        foreach (var permission in user.Permissions)
        {
            claims.Add(new Claim(ClaimTypeConstants.PermissionClaimType, permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtConfig.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: this.jwtConfig.Issuer,
            audience: this.jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(this.jwtConfig.ExpirationInMinutes, CultureInfo.InvariantCulture)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> IsTokenBlacklisted(int tokenId)
    {
        var token = await this.context.JwtTokens.FindAsync(tokenId) ?? throw new TokenNotFoundException(tokenId);
        return token.Blacklisted;
    }
}
