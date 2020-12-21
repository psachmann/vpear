// <copyright file="PostRegisterValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the post register request data.
    /// </summary>
    public class PostRegisterValidator : AbstractValidator<PostRegisterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRegisterValidator"/> class.
        /// </summary>
        public PostRegisterValidator()
        {
            this.When(r => r.DisplayName != null, () =>
            {
                this.RuleFor(r => r.DisplayName)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(Limits.MinStringLength)
                    .MaximumLength(Limits.MaxStringLength)
                    .OverridePropertyName("display_name");
            });

            this.RuleFor(r => r.Email)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Limits.MaxStringLength)
                .EmailAddress()
                .OverridePropertyName("email");

            this.RuleFor(r => r.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(Limits.MinPasswordLength)
                .MaximumLength(Limits.MaxPasswordLength)
                .OverridePropertyName("password");
        }
    }
}
