// <copyright file="DeviceStatusChangedEventDetectorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Data;
using VPEAR.Server.Data.EventDetectors;
using Xunit;

namespace VPEAR.Server.Test
{
    [Collection("RepositoryTest")]
    public class DeviceStatusChangedEventDetectorTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly IRepository<Device, Guid> devices;

        public DeviceStatusChangedEventDetectorTest(VPEARDbContextFixture fixture)
        {
            var context = fixture.Context;
            var logger = Mocks.CreateLogger<IRepository<Device, Guid>>();

            this.devices = new Repository<VPEARDbContext, Device, Guid>(context, logger);
        }
    }
}
