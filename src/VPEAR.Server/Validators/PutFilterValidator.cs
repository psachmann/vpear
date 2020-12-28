// <copyright file="PutFilterValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutFilterRequest"/> request body.
    /// This request don't need validation, but to prevent an unresolved
    /// exception from the FluenteValidation.AspNetCore extension we will
    /// register this dummy validator with the dependency injection container.
    /// </summary>
    public class PutFilterValidator : AbstractValidator<PutFilterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutFilterValidator"/> class.
        /// </summary>
        public PutFilterValidator()
        {
        }
    }
}
