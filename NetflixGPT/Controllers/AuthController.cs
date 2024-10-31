using System;
using Microsoft.AspNetCore.Mvc;
using NetflixGPT.infra.DB.Entities;
using NetflixGPT.Infra.DB.Dto;
using NetflixGPT.Services.Authentication;
using static NetflixGPT.Authentication.AuthenticationService;

namespace NetflixGPT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var token = await _authService.Authenticate(username:username,password: password);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] UserDto newUser)
    {
        try
        {
        
            await _authService.RegisterUser(newUser);

            var token = await _authService.Authenticate(newUser.Username, newUser.Password);

            if (token == null)
            {
                return Unauthorized("User could not be authenticated after signup.");
            }

            return Ok(new { Message = "Signup successful", Token = token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred during signup: {ex.Message}");
        }
    }
}

