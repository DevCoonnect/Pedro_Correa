using AuthMuseum.Core.Helpers;
using AuthMuseum.Core.Services;
using AuthMuseum.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMuseum.Application.Controllers;

[ApiController]
[Route("api")]
public class UserController(IUserService userService) : ControllerBase
{

    [HttpPost("user")]
    //[Authorize("Bearer", Roles = "ADMIN")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var result = await userService.CreateUserAsync(request);
        
        return result.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }
    
    [HttpGet("user/{id:int}")]
    [Authorize("Bearer", Roles = "ADMIN")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var result = await userService.GetUserByIdAsync(id);
        return result.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }
    
    [HttpPost("user/{id:int}")]
    public async Task<IActionResult> UpdateUserAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("user/{id:int}")]
    //[Authorize("Bearer", Roles = "ADMIN")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var result = await userService.DeleteUserAsync(id);

        return result.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }
}