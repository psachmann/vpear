// <copyright file="PutTokenResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class PutTokenResponse
    {
        [JsonPropertyName("new_token")]
        public string NewToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_at")]
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
