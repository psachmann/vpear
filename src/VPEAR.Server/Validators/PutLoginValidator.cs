// <copyright file="PutLoginValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutLoginRequest"/> request body.
    /// </summary>
    public class PutLoginValidator : AbstractValidator<PutLoginRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutLoginValidator"/> class.
        /// </summary>
        public PutLoginValidator()
        {
            this.RuleFor(request => request.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Limits.MaxStringLength)
                .OverridePropertyName("name");

            this.RuleFor(request => request.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(Limits.MinPasswordLength)
                .MaximumLength(Limits.MaxPasswordLength)
                .OverridePropertyName("password");
        }
    }
}
