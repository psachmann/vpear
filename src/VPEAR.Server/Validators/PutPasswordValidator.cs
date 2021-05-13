// <copyright file="PutPasswordValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutPasswordRequest"/> request body.
    /// </summary>
    public class PutPasswordValidator : AbstractValidator<PutPasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutPasswordValidator"/> class.
        /// </summary>
        public PutPasswordValidator()
        {
            this.RuleFor(request => request.NewPassword)
                .NotNull()
                .NotEmpty()
                .MinimumLength(Limits.MinPasswordLength)
                .MaximumLength(Limits.MaxPasswordLength)
                .OverridePropertyName("new_password");

            this.RuleFor(request => request.OldPassword)
                .NotNull()
                .NotEmpty()
                .MinimumLength(Limits.MinPasswordLength)
                .MaximumLength(Limits.MaxPasswordLength)
                .OverridePropertyName("old_password");

            this.RuleFor(request => request.Token)
                .NotNull()
                .NotEmpty()
                .OverridePropertyName("token");

            this.RuleFor(request => request)
                .Must(request => request.NewPassword != request.OldPassword)
                .WithMessage("New and old password should be different.");
        }
    }
}
