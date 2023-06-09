﻿using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions;

public static class UserManagerExtension
{
    public static async Task<AppUser> FindByEmailWithAddressAsync(
        this UserManager<AppUser> input, ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);
        var userByEmail = await input.Users
            .Include(x => x.Address)
            .SingleOrDefaultAsync(x => x.Email == email);
        return userByEmail;
    }

    public static async Task<AppUser> FindByEmailFromClaimsPrincipal(
               this UserManager<AppUser> input, ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);
        return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
    }
}
