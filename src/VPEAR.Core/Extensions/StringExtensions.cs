// <copyright file="StringExtensions.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json;

namespace VPEAR.Core.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the given json string in an object of type T.
        /// </summary>
        /// <param name="json">The json string to convert.</param>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <returns>An object of type T.</returns>
        public static T FromJsonString<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
