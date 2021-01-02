// <copyright file="IPAddressExtensions.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net;
using System.Net.Sockets;

namespace VPEAR.Core.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="IPAddress"/> type.
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
            byte[] addressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (addressBytes.Length != subnetMaskBytes.Length)
            {
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
            }

            byte[] broadcastAddress = new byte[addressBytes.Length];
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
            byte[] addressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (addressBytes.Length != subnetMaskBytes.Length)
            {
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
            }

            byte[] broadcastAddress = new byte[addressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(addressBytes[i] & subnetMaskBytes[i]);
            }

            return new IPAddress(broadcastAddress);
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
    }
}
