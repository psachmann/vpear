// <copyright file="PutDeviceValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutDeviceRequest"/> request body.
    /// </summary>
    public class PutDeviceValidator : AbstractValidator<PutDeviceRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutDeviceValidator"/> class.
        /// </summary>
        public PutDeviceValidator()
        {
            this.When(request => request.DisplayName != null, () =>
            {
                this.RuleFor(request => request.DisplayName)
                    .NotNull()
                    .NotEmpty()
                    .MaximumLength(Limits.MaxStringLength)
                    .OverridePropertyName("display_name");
            });

            this.When(request => request.Frequency != null, () =>
            {
                this.RuleFor(request => request.Frequency)
                    .NotNull()
                    .InclusiveBetween(1, int.MaxValue)
                    .OverridePropertyName("frequency");
            });

            this.When(request => request.RequiredSensors != null, () =>
            {
                this.RuleFor(request => request.RequiredSensors)
                    .NotNull()
                    .InclusiveBetween(1, int.MaxValue)
                    .OverridePropertyName("required_sensors");
            });
        }
    }
}
