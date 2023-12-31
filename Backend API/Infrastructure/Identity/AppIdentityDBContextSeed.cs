﻿using Backend_API.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppIdentityDBContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            AppUser user = new AppUser()
            {
                DisplayName = "Bob",
                Email = "bob@yahoo.com",
                UserName = "Bob@test.com",
                Address = new Address()
                {
                    FirstName = "Bob",
                    LastName = "Bobbity",
                    Street = "10 The Street",
                    City = "New York",
                    State = "NY",
                    ZipCode = "90210"
                }
            };
            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }
}