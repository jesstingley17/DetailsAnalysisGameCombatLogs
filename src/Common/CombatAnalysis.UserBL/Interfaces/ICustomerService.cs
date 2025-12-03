using CombatAnalysis.UserBL.DTO;

namespace CombatAnalysis.UserBL.Interfaces;

public interface ICustomerService : IService<CustomerDto, string>
{
    Task UpdateAsync(string id, CustomerDto item);
}
