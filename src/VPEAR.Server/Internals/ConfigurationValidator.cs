// <copyright file="ConfigurationValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Internals
{
    internal class ConfigurationValidator : AbstractValidator<Configuration>
    {
        public ConfigurationValidator()
        {
            RuleFor(c => c.DatabaseConnection)
                .NotNull()
                .NotEmpty();

            When(c => c.HttpPort != Defaults.DefaultHttpPort, () =>
            {
                RuleFor(c => c.HttpPort)
                    .GreaterThanOrEqualTo(1024)
                    .LessThan(65536);
            });

            When(c => c.HttpsPort != Defaults.DefaultHttpsPort, () =>
            {
                RuleFor(c => c.HttpsPort)
                    .GreaterThanOrEqualTo(1024)
                    .LessThan(65536);
            });
        }
    }
}
