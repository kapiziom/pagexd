﻿using Microsoft.AspNetCore.Identity;
using pagexd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Data
{
    public static class IdentityDataInit
    {
        public static void SeedData(UserManager<PageUser> userManager, RoleManager<PageRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<PageUser> userManager)
        {
            if (userManager.FindByNameAsync("user001").Result == null)
            {
                PageUser user = new PageUser();
                user.UserName = "user001";
                user.Email = "us1@x.d";
                user.AccInfo = "test account";

                IdentityResult result = userManager.CreateAsync
                (user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"NormalUser").Wait();
                }
            }

            if (userManager.FindByNameAsync("user002").Result == null)
            {
                PageUser user = new PageUser();
                user.UserName = "user002";
                user.Email = "us2@x.d";
                user.AccInfo = "test acc 2";

                IdentityResult result = userManager.CreateAsync
                (user, "P@ssw0rd").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                }
            }

            if (userManager.FindByNameAsync("admin").Result == null)
            {
                PageUser user = new PageUser();
                user.UserName = "admin";
                user.Email = "xd@xd.xd";
                user.AccInfo = "administrator account";

                IdentityResult result = userManager.CreateAsync
                (user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<PageRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                PageRole role = new PageRole();
                role.Name = "NormalUser";
                role.Description = "Perform normal operations.";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Banned").Result)
            {
                PageRole role = new PageRole();
                role.Name = "Banned";
                role.Description = "can not add any content";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                PageRole role = new PageRole();
                role.Name = "Administrator";
                role.Description = "Perform all the operations.";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
