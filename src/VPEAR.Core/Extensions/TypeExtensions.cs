// <copyright file="TypeExtensions.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json;

namespace VPEAR.Core.Extensions
{
    public static class TypeExtensions
    {
        public static T? Clone<T>(this T source)
        {
            var json = JsonSerializer.Serialize(source);

            return JsonSerializer.Deserialize<T>(json);
        }

        public static string? ToJsonString<T>(this T source)
        {
            return JsonSerializer.Serialize(source);
        }
    }
}
