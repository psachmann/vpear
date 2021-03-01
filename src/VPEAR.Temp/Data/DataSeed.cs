// <copyright file="DataSeed.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// This class contains the seed data for the database.
    /// </summary>
    public static class DataSeed
    {
        static DataSeed()
        {
            Devices = new List<Device>();
            Filters = new List<Filter>();
            Frames = new List<Frame>();

            foreach (var i in Enumerable.Range(1, 8))
            {
                var id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)i);

                var device = new Device()
                {
                    Address = $"http://192.168.178.{i}",
                    Class = "Boditrak DataPort",
                    DisplayName = $"Boditrak DataPort {i}",
                    Frames = new List<Frame>(),
                    Frequency = i * 100,
                    Id = id,
                    Name = $"DataPort-{i}",
                    RequiredSensors = i,
                    Status = ((DeviceStatus)(i % 4)) == DeviceStatus.Recording ? DeviceStatus.Stopped : (DeviceStatus)(i % 4),
                };

                var filter = new Filter()
                {
                    DeviceForeignKey = id,
                    Id = id,
                    Noise = true,
                    Smooth = true,
                    Spot = true,
                };

                var frame = new Frame()
                {
                    DeviceForeignKey = id,
                    FilterForeignKey = id,
                    Id = id,
                    Index = i,
                    Readings = new List<IList<int>>()
                    {
                        new List<int> { 0, 0, 0, 0, 0 },
                        new List<int> { 0, 0, 0, 0, 0 },
                        new List<int> { 0, 0, 0, 0, 0 },
                        new List<int> { 0, 0, 0, 0, 0 },
                        new List<int> { 0, 0, 0, 0, 0 },
                    },
                    Time = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                };

                Devices.Add(device);
                Filters.Add(filter);
                Frames.Add(frame);
            }
        }

        /// <summary>
        /// Gets the device seed data.
        /// </summary>
        /// <value>The database seed for <see cref="Device"/>.</value>
        public static IList<Device> Devices { get; }

        /// <summary>
        /// Gets the filter seed data.
        /// </summary>
        /// <value>The database seed for <see cref="Filter"/>.</value>
        public static IList<Filter> Filters { get; }

        /// <summary>
        /// Gets the frame seed data.
        /// </summary>
        /// <value>The database seed for <see cref="Frame"/>.</value>
        public static IList<Frame> Frames { get; }

        /// <summary>
        /// Seeds roles and users into the db.
        /// </summary>
        /// <param name="roles">The role manager to seed roles.</param>
        /// <param name="users">The user manager to seed users.</param>
        public static void Seed(RoleManager<IdentityRole> roles, UserManager<IdentityUser> users)
        {
            SeedRoles(roles);
            SeedUsers(users);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roles)
        {
            foreach (var roleName in Roles.AllRoles)
            {
                if (!roles.RoleExistsAsync(roleName).Result)
                {
                    var role = new IdentityRole(roleName);

                    _ = roles.CreateAsync(role).Result;
                }
            }
        }

        private static void SeedUsers(UserManager<IdentityUser> users)
        {
            var adminName = Defaults.DefaultAdminName;
            var adminPassword = Defaults.DefaultAdminPassword;

            if (users.FindByNameAsync(adminName).Result == null)
            {
                var admin = new IdentityUser()
                {
                    EmailConfirmed = true,
                    UserName = adminName,
                };

                var result = users.CreateAsync(admin, adminPassword).Result;

                if (result.Succeeded)
                {
                    _ = users.AddToRolesAsync(admin, Roles.AllRoles).Result;
                }
            }
        }
    }
}
