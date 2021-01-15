// <copyright file="SkipIfNoDeviceFactAttribute.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using VPEAR.Core;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class SkipIfNoDeviceFactAttribute : FactAttribute
    {
        public SkipIfNoDeviceFactAttribute(string baseAddress)
        {
            using var httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(Defaults.DefaultHttpTimeout),
            };
            using var client = new DeviceClient(baseAddress, httpClient);

            if (!client.CanConnectAsync().Result)
            {
                this.Skip = "Device not reachable";
            }
        }
    }
}
