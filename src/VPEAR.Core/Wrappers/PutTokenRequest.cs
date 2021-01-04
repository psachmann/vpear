// <copyright file="PutTokenRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class PutTokenRequest
    {
        [JsonPropertyName("old_token")]
        public string OldToken { get; set; }
    }
}
