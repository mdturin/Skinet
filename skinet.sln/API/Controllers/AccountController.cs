﻿using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class AccountController : BaseController
{
    #region private members
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    #endregion

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
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
        var user = await _userManager
            .FindByEmailFromClaimsPrincipal(HttpContext.User);
        return Ok(CreateUserDTO(user));
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
        var user = await _userManager
            .FindByEmailWithAddressAsync(HttpContext.User);
        return Ok(_mapper.Map<Address, AddressDTO>(user.Address));
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
    {
        var user = await _userManager
            .FindByEmailWithAddressAsync(HttpContext.User);
        user.Address = _mapper.Map<AddressDTO, Address>(address);
        var result = await _userManager.UpdateAsync(user);
        if(result.Succeeded)
            return Ok(_mapper.Map<Address, AddressDTO>(user.Address));
        return BadRequest("Problem updating the user");
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if(user == null) return Unauthorized(new ApiResponse(401));

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);

        if(!result.Succeeded)
            return Unauthorized(new ApiResponse(401));

        return Ok(CreateUserDTO(user));
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
        {
            ApiValidationErrorResponse validationErrorResponse = new()
            {
                Errors = new[] { "Email address is in use" }
            };

            return new BadRequestObjectResult(validationErrorResponse);
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if(!result.Succeeded) return BadRequest(new ApiResponse(400));

        return Ok(CreateUserDTO(user));
    }

    #region private methods
    private UserDTO CreateUserDTO(AppUser user)
    {
        return new UserDTO
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }
    #endregion
}
