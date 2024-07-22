using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using Backend.GraphQL.Exceptions;
using Backend.GraphQL.Models;
using Backend.Models;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Backend.GraphQL;

public class Mutation
{

    [Error<FailedLoginException>]
    public LoginRes Login(string email, string password, [Service] TodoDbContext context, [Service] IConfiguration configuration)
    {
        var user = context.Users.SingleOrDefault(u => u.Email == email) ?? throw new FailedLoginException();

        var hash = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(
            password,
            Convert.FromBase64String(user.PasswordSalt),
            100000,
            HashAlgorithmName.SHA256,
            32
        ));

        if (user.Password != hash)
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
                new(ClaimTypes.NameIdentifier, user.Id),
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

    [Error<InvalidEmailException>]
    [Error<EmailInUseException>]
    public User RegisterUser(string email, string password, [Service] TodoDbContext context)
    {
        try
        {

            _ = new MailAddress(email);
        }
        catch
        {
            throw new InvalidEmailException();
        }

        if (context.Users.Any(u => u.Email == email))
        {
            throw new EmailInUseException();
        }

        var salt = RandomNumberGenerator.GetBytes(16);
        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        ));

        context.Users.Add(
            new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                Password = hash,
                PasswordSalt = Convert.ToBase64String(salt),
            }
        );
        context.SaveChanges();

        return context.Users.Single(u => u.Email == email);
    }

    [Authorize]
    public Backend.Models.Task CreateTask(string title, string description, bool isComplete, [Service] TodoDbContext context, ClaimsPrincipal claims)
    {
        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();

        var task = new Backend.Models.Task
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            Description = description,
            IsComplete = isComplete,
            UserId = userId,
        };

        context.Task.Add(task);
        context.SaveChanges();

        return task;
    }
}