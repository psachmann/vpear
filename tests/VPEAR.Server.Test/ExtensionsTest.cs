// <copyright file="ExtensionsTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;
using VPEAR.Core.Extensions;
using Xunit;

namespace VPEAR.Server.Test
{
    public class ExtensionsTest
    {
        [Fact]
        public void GetBroadcastAddressTest()
        {
            var address = IPAddress.Parse("192.168.2.234");
            var subnetMask = IPAddress.Parse("255.255.255.0");
            var expectedBroadcastAddress = IPAddress.Parse("192.168.2.255");
            var broadcastAddress = address.GetBroadcastAddress(subnetMask);

            Assert.True(expectedBroadcastAddress.Equals(broadcastAddress), "The IP address should be equals.");
        }

        [Fact]
        public void GetNetworkAddressTest()
        {
            var address = IPAddress.Parse("192.168.2.234");
            var subnetMask = IPAddress.Parse("255.255.255.0");
            var expectedNetworkAddress = IPAddress.Parse("192.168.2.0");
            var networkAddress = address.GetNetworkAddress(subnetMask);

            Assert.True(expectedNetworkAddress.Equals(networkAddress), "The IP address should be equals.");
        }

        [Fact]
        public void IsIPv4TrueTest()
        {
            var address = IPAddress.Parse("0.0.0.0");

            Assert.True(address.IsIPv4(), "This should be a valid IP v4 address.");
        }

        [Fact]
        public void IsIPv4FalseTest()
        {
            var address = IPAddress.Parse("0::0");

            Assert.False(address.IsIPv4(), "This should NOT be a valid IP v4 address.");
        }
    }
}
