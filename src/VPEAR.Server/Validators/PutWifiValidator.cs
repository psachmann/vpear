// <copyright file="PutWifiValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutWifiRequest"/> request body.
    /// </summary>
    public class PutWifiValidator : AbstractValidator<PutWifiRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutWifiValidator"/> class.
        /// </summary>
        public PutWifiValidator()
        {
            throw new System.NotImplementedException();
        }
    }
}
