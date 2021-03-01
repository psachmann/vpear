// <copyright file="PutUserValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutUserRequest"/> request body.
    /// </summary>
    public class PutUserValidator : AbstractValidator<PutUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutUserValidator"/> class.
        /// </summary>
        public PutUserValidator()
        {
            this.When(request => request.NewPassword != null || request.OldPassword != null, () =>
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

                this.RuleFor(request => request)
                    .Must(request => request.NewPassword != request.OldPassword);
            });
        }
    }
}
