using AdminPanel.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Shared.ControllerBases;

[ApiController]
[Route("api/[controller]")]
public class CustomBaseController : ControllerBase
{
    protected static IActionResult CreateActionResultInstance<T>(Response<T> response)
    {
        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}