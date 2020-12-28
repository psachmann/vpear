// <copyright file="EndpointTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class EndpointTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Get = "GET";
        private const string Put = "PUT";
        private const string Post = "POST";
        private const string Delete = "DELETE";
        private readonly WebApplicationFactory<Startup> factory;

        public EndpointTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData(Routes.BaseRoute, Get)]
        [InlineData(Routes.DeviceRoute, Get)]
        [InlineData(Routes.DeviceRoute, Put)]
        [InlineData(Routes.DeviceRoute, Post)]
        [InlineData(Routes.DeviceRoute, Delete)]
        [InlineData(Routes.SensorsRoute, Get)]
        [InlineData(Routes.FramesRoute, Get)]
        [InlineData(Routes.FilterRoute, Get)]
        [InlineData(Routes.FilterRoute, Put)]
        [InlineData(Routes.PowerRoute, Get)]
        [InlineData(Routes.WifiRoute, Get)]
        [InlineData(Routes.WifiRoute, Put)]
        [InlineData(Routes.FirmwareRoute, Get)]
        [InlineData(Routes.FirmwareRoute, Put)]
        [InlineData(Routes.UsersRoute, Get)]
        [InlineData(Routes.UsersRoute, Put)]
        [InlineData(Routes.UsersRoute, Delete)]
        [InlineData(Routes.RegisterRoute, Post)]
        [InlineData(Routes.LoginRoute, Put)]
        public async Task EndpointExistenceTest(string url, string method)
        {
            var client = this.factory.CreateClient();
            var response = method switch
            {
                Get => await client.GetAsync(url),
                Put => await client.PutAsync(url, new StringContent("{}")),
                Post => await client.PostAsync(url, new StringContent("{}")),
                Delete => await client.DeleteAsync(url),
                _ => throw new ArgumentOutOfRangeException(nameof(method)),
            };

            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
