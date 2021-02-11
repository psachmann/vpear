// <copyright file="ConfigurationValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using System;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="Configuration"/> from 'appsettings.json'.
    /// </summary>
    public class ConfigurationValidator : AbstractValidator<Configuration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidator"/> class.
        /// </summary>
        public ConfigurationValidator()
        {
            this.RuleFor(c => c.DbConnection)
                .NotNull()
                .NotEmpty();

            this.RuleFor(c => c.DbVersion)
                .NotNull()
                .NotEmpty()
                .Must(dbVersion => Version.TryParse(dbVersion, out _));

            this.When(c => c.HttpPort != Defaults.DefaultHttpPort, () =>
            {
                this.RuleFor(c => c.HttpPort)
                    .GreaterThanOrEqualTo(1024)
                    .LessThan(65536);
            });

            this.When(c => c.HttpsPort != Defaults.DefaultHttpsPort, () =>
            {
                this.RuleFor(c => c.HttpsPort)
                    .GreaterThanOrEqualTo(1024)
                    .LessThan(65536);
            });

            this.RuleFor(c => c.Secret)
                .NotNull()
                .NotEmpty()
                .MinimumLength(Limits.MinSecretLength);
        }
    }
}
