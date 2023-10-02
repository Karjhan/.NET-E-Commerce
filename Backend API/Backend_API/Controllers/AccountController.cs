using System.Security.Claims;
using AutoMapper;
using Backend_API.DTO;
using Backend_API.Entities.Identity;
using Backend_API.Errors;
using Backend_API.Extensions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers;

public class AccountController : BaseAPIController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        AppUser user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
        return new UserDTO()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDTO>> GetUserAddress()
    {
        AppUser user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);
        return _mapper.Map<Address,AddressDTO>(user.Address);
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDto)
    {
        AppUser user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);
        user.Address = _mapper.Map<AddressDTO, Address>(addressDto);
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return _mapper.Map<Address, AddressDTO>(user.Address);
        }
        return BadRequest(new APIResponse(500));
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        AppUser? user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            return Unauthorized(new APIResponse(401));
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new APIResponse(401));
        }
        return new UserDTO()
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        AppUser user = new AppUser()
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new APIResponse(400));
        }
        return new UserDTO()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }
}