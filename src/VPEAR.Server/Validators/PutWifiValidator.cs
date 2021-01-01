// <copyright file="PutWifiValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

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
            this.When(request => request.Ssid == null, () =>
            {
                this.RuleFor(request => request.Password)
                    .Null()
                    .OverridePropertyName("password");
            });

            this.When(request => request.Password == null, () =>
            {
                this.RuleFor(request => request.Ssid)
                    .Null()
                    .OverridePropertyName("ssid");
            });

            this.When(request => request.Ssid != null && request.Password != null, () =>
            {
                this.RuleFor(request => request.Ssid)
                    .NotNull()
                    .NotEmpty()
                    .OverridePropertyName("ssid");

                this.RuleFor(request => request.Password)
                    .NotNull()
                    .NotEmpty()
                    .OverridePropertyName("password");
            });

            this.When(request => request.Mode != null, () =>
            {
                this.RuleFor(request => request.Mode)
                    .NotNull()
                    .NotEmpty()
                    .Must(mode => WifiModes.All.Contains(mode))
                    .OverridePropertyName("mode");
            });
        }
    }
}
