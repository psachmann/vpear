// <copyright file="SkipIfNoDbOrDeviceTheoryAttribute.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using VPEAR.Core;
using VPEAR.Server.Data;
using Xunit;

namespace VPEAR.Server.Test
{
    public class SkipIfNoDbOrDeviceTheoryAttribute : TheoryAttribute
    {
        private static readonly DesignTimeVPEARDbContextFactory ContextFactory = new DesignTimeVPEARDbContextFactory();

        public SkipIfNoDbOrDeviceTheoryAttribute(string baseAddress)
        {
            using var context = ContextFactory.CreateDbContext(Environment.GetCommandLineArgs());
            using var client = new DeviceClient(baseAddress, new HttpClient());

            if (!context.Database.CanConnect() || client.CanConnectAsync().Result)
            {
                this.Skip = "Can not connect to database or device.";
            }
        }
    }
}
