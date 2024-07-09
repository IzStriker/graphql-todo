using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.GraphQL.Exceptions;
using Backend.GraphQL.Models;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.GraphQL;

public class Mutation
{

    [Error<FailedLoginException>]
    public LoginRes Login(string email, string password, [Service] TodoDbContext context, [Service] IConfiguration configuration)
    {
        var user = context.Users.Single(u => u.Email == email);

        // var hash = Rfc2898DeriveBytes.Pbkdf2(
        //     Encoding.UTF8.GetBytes(input.Password),
        //     Encoding.UTF8.GetBytes("ChangeMe!!"),
        //     350000,
        //     HashAlgorithmName.SHA256,
        //     64
        // );

        if (user is null || password != user.Password)
        {
            throw new FailedLoginException();
        }

        var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? throw new Exception("No JWT key provided"))
            );

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(1),
            claims:
            [
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ],
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new LoginRes
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ValidTill = token.ValidTo
        };
    }
}