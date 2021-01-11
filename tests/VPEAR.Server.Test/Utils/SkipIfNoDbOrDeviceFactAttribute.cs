// <copyright file="SkipIfNoDbOrDeviceFactAttribute.cs" company="Patrick Sachmann">
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
    public class SkipIfNoDbOrDeviceFactAttribute : FactAttribute
    {
        private static readonly DesignTimeVPEARDbContextFactory ContextFactory = new DesignTimeVPEARDbContextFactory();

        public SkipIfNoDbOrDeviceFactAttribute(string baseAddress)
        {
            using var context = ContextFactory.CreateDbContext(Environment.GetCommandLineArgs());
            using var client = new DeviceClient(baseAddress, CreateClient());

            if (!context.Database.CanConnect() || client.CanConnectAsync().Result)
            {
                this.Skip = "Can not connect to database or device.";
            }
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(300.0),
            };

            return client;
        }
    }
}
