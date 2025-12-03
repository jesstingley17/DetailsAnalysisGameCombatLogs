using Chat.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatAPI.Core;

public static class ExtractDomainStatusCode
{
    public static IActionResult ExtractDomainCode(this ControllerBase controller, ExceptionCode code)
    {
        return code switch
        {
            ExceptionCode.DomainError => controller.BadRequest(),
            ExceptionCode.NotFound => controller.NotFound(),
            ExceptionCode.Forbidden => controller.Forbid(),
            _ => controller.StatusCode(500, "Internal server error."),
        };
    }
}