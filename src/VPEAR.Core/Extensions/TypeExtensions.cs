// <copyright file="TypeExtensions.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json;

namespace VPEAR.Core.Extensions
{
    /// <summary>
    /// Extension methods for all types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Makes a deep copy of the given object.
        /// </summary>
        /// <param name="source">The clone source.</param>
        /// <typeparam name="T">The clone type.</typeparam>
        /// <returns>The cloned object.</returns>
        public static T Clone<T>(this T source)
        {
            var json = JsonSerializer.Serialize(source);

            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Makes a json string from the given object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <typeparam name="T">The source type.</typeparam>
        /// <returns>A json string.</returns>
        public static string ToJsonString<T>(this T source)
        {
            return JsonSerializer.Serialize(source);
        }
    }
}
