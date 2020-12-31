// <copyright file="PostDeviceValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using System.Net;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PostDeviceRequest"/> request body.
    /// </summary>
    public class PostDeviceValidator : AbstractValidator<PostDeviceRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostDeviceValidator"/> class.
        /// </summary>
        public PostDeviceValidator()
        {
            this.RuleFor(request => request.Address)
                .NotNull()
                .NotEmpty()
                .Must(ip => IPAddress.TryParse(ip, out _))
                .OverridePropertyName("start_ip")
                .WithMessage("'start_ip' is NOT a valid IP address.");

            this.RuleFor(request => request.SubnetMask)
                .NotNull()
                .NotEmpty()
                .Must(ip => IPAddress.TryParse(ip, out _))
                .OverridePropertyName("stop_ip")
                .WithMessage("'stop_ip' is NOT a valid IP address.");
        }
    }
}
