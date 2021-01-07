// <copyright file="IPAddressExtensions.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace VPEAR.Core.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="IPAddress"/> type.
    /// </summary>
    public static class IPAddressExtensions
    {
        /// <summary>
        /// Calculates the broadcast address for the given address and subnet mask.
        /// </summary>
        /// <param name="address">The address in the subnet.</param>
        /// <param name="subnetMask">The mask for the subnet.</param>
        /// <returns>The broadcast address.</returns>
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            var addressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (addressBytes.Length != subnetMaskBytes.Length)
            {
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
            }

            var broadcastAddress = new byte[addressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(addressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            return new IPAddress(broadcastAddress);
        }

        /// <summary>
        /// Calculates the network address for the given address and subnet mask.
        /// </summary>
        /// <param name="address">The address in the subnet.</param>
        /// <param name="subnetMask">The mask for the subnet.</param>
        /// <returns>The network address.</returns>
        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            var addressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (addressBytes.Length != subnetMaskBytes.Length)
            {
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
            }

            var networkAddress = new byte[addressBytes.Length];
            for (int i = 0; i < networkAddress.Length; i++)
            {
                networkAddress[i] = (byte)(addressBytes[i] & subnetMaskBytes[i]);
            }

            return new IPAddress(networkAddress);
        }

        /// <summary>
        /// Checks if the given address is an IP v4 address.
        /// </summary>
        /// <param name="address">The address to check.</param>
        /// <returns>True if address is IP v4, otherwise false.</returns>
        public static bool IsIPv4(this IPAddress address)
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the given subnet mask is a valid IP v4 subnet mask.
        /// </summary>
        /// <param name="subnetMask">The subnet mask to check.</param>
        /// <returns>True if subnetMask is a valid. otherwise false.</returns>
        public static bool IsIPv4SubnetMask(this IPAddress subnetMask)
        {
            if (!subnetMask.IsIPv4())
            {
                throw new ArgumentException("Is not IP v4 address.", nameof(subnetMask));
            }

            // NOTE: Unity doesn't support .Net Standard 2.1 and .Net Standard 2.0 doesn't
            // contains BitArray.LeftSchift() or BitArray.RightShift()
            var possibleSubnetMasks = new List<string>()
            {
                "255.0.0.0",
                "255.128.0.0",
                "255.192.0.0",
                "255.224.0.0",
                "255.240.0.0",
                "255.252.0.0",
                "255.254.0.0",
                "255.255.0.0",
                "255.255.128.0",
                "255.255.192.0",
                "255.255.224.0",
                "255.255.240.0",
                "255.255.252.0",
                "255.255.254.0",
                "255.255.255.0",
                "255.255.255.128",
                "255.255.255.192",
                "255.255.255.224",
                "255.255.255.240",
                "255.255.255.252",
                "255.255.255.254",
                "255.255.255.255",
            };

            return possibleSubnetMasks.Contains(subnetMask.ToString());
        }
    }
}
