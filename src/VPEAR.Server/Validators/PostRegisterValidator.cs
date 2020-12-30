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
    /// Validates the <see cref="PostRegisterRequest"/> request body.
    /// </summary>
    public class PostRegisterValidator : AbstractValidator<PostRegisterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRegisterValidator"/> class.
        /// </summary>
        public PostRegisterValidator()
        {
            this.When(request => request.DisplayName != null, () =>
            {
                this.RuleFor(request => request.DisplayName)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(Limits.MinStringLength)
                    .MaximumLength(Limits.MaxStringLength)
                    .OverridePropertyName("display_name");
            });

            this.RuleFor(request => request.Email)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Limits.MaxStringLength)
                .EmailAddress()
                .OverridePropertyName("email");

            this.RuleFor(request => request.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(Limits.MinPasswordLength)
                .MaximumLength(Limits.MaxPasswordLength)
                .OverridePropertyName("password");
        }
    }
}
