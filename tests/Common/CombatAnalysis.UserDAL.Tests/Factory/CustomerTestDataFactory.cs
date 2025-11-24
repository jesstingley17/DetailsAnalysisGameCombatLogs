using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserDAL.Tests.Factory;

internal static class CustomerTestDataFactory
{
    public static Customer Create(
        string? id = null,
        string? country = null,
        string? city = null,
        int? postalCode = null,
        string? appUserId = null
        )
    {
        var customer = new Customer(
            Id: id ?? "uid-22",
            Country: country ?? "country",
            City: city ?? "city",
            PostalCode: postalCode ?? 123123,
            AppUserId: appUserId ?? "uid-23"
        );

        return customer;
    }

    public static Customer[] CreateCollection(
        int size = 3 
        )
    {
        var collection = new Customer[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new Customer(
                Id: $"uid-{i}",
                Country: $"country-{i}",
                City: $"city-{i}",
                PostalCode: 123123 + i,
                AppUserId: $"uid-1-{i}"
            );
        }

        return collection;
    }
}
