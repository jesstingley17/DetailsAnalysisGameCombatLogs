using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class AuthCodeService(IPkeRepository authCodeRepository) : IAuthCodeService
{
    private readonly IPkeRepository _authCodeRepository = authCodeRepository;

    public async Task RemoveExpiredCodesAsync()
    {
        await _authCodeRepository.RemoveExpiredCodesAsync();
    }
}
