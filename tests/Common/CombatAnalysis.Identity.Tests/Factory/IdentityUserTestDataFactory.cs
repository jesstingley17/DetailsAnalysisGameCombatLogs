using CombatAnalysis.Identity.DTO;
using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.Identity.Tests.Factory;

internal static class IdentityUserTestDataFactory
{
    public static IdentityUser Create(
        string? id = null,
        string? email = null,
        bool? emailVerified = null,
        string? passwordHash = null,
        string? salt = null
        )
    {
        var customer = new IdentityUser
        {
            Id = id ?? "uid-1",
            Email = email ?? "email",
            EmailVerified = emailVerified ?? false,
            PasswordHash = passwordHash ?? "has-uid-1",
            Salt = salt ?? "uid-2"
        };

        return customer;
    }

    public static IdentityUserDto CreateDto(
        string? id = null,
        string? email = null,
        bool? emailVerified = null,
        string? passwordHash = null,
        string? salt = null
        )
    {
        var customer = new IdentityUserDto
        {
            Id = id ?? "uid-1",
            Email = email ?? "email",
            EmailVerified = emailVerified ?? false,
            PasswordHash = passwordHash ?? "has-uid-1",
            Salt = salt ?? "uid-2"
        };

        return customer;
    }

    public static IdentityUser[] CreateCollection(
        int size = 3 
        )
    {
        var collection = new IdentityUser[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new IdentityUser
            {
                Id = $"uid-1-{i}",
                Email = $"email-{i}",
                EmailVerified = false,
                PasswordHash = $"has-uid-1={i}",
                Salt = $"uid-2-{i}"
            };
        }

        return collection;
    }

    public static IdentityUserDto[] CreateDtoCollection(
        int size = 3
        )
    {
        var collection = new IdentityUserDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new IdentityUserDto
            {
                Id = $"uid-1-{i}",
                Email = $"email-{i}",
                EmailVerified = false,
                PasswordHash = $"has-uid-1={i}",
                Salt = $"uid-2-{i}"
            };
        }

        return collection;
    }
}
