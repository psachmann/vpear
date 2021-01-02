// <copyright file="StringExtensions.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json;

namespace VPEAR.Core.Extensions
{
    public static class StringExtensions
    {
        public static T? FromJsonString<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source);
        }
    }
}
