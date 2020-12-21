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
    /// Validates the put user request data.
    /// </summary>
    public class PutUserValidator : AbstractValidator<PutUserRequest>
    {
        public PutUserValidator()
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

            this.When(r => r.Email != null, () =>
            {
                this.RuleFor(r => r.Email)
                    .NotNull()
                    .NotEmpty()
                    .MaximumLength(Limits.MaxStringLength)
                    .EmailAddress()
                    .OverridePropertyName("email");
            });

            this.When(r => r.IsVerified, () =>
            {
                this.RuleFor(r => r.Role)
                    .NotNull()
                    .NotEmpty()
                    .Must(r => Roles.AllRoles.Contains(r!))
                    .OverridePropertyName("role");
            });

            this.When(r => r.Password != null, () =>
            {
                this.RuleFor(r => r.Password)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(Limits.MinPasswordLength)
                    .MaximumLength(Limits.MaxPasswordLength)
                    .OverridePropertyName("password");
            });

            this.When(r => r.Role != null, () =>
            {
                this.RuleFor(r => r.Role)
                    .NotNull()
                    .NotEmpty()
                    .Must(r => Roles.AllRoles.Contains(r!))
                    .OverridePropertyName("role")
                    .DependentRules(() =>
                    {
                        this.RuleFor(r => r)
                            .Must(r => r.IsVerified);
                    });
            });
        }
    }
}
