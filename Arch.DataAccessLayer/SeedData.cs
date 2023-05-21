using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.DataAccessLayer
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Admin rolünü oluştur
            await CreateRoleIfNotExists(roleManager, AppRole.Admin);

            // Designer rolünü oluştur
            await CreateRoleIfNotExists(roleManager, AppRole.Designer);

            // Customer rolünü oluştur
            await CreateRoleIfNotExists(roleManager, AppRole.Customer);
        }

        private static async Task CreateRoleIfNotExists(RoleManager<AppRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole { Name = roleName };
                await roleManager.CreateAsync(role);
            }
        }
    }

}
