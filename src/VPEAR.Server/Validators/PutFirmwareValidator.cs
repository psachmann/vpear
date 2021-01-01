// <copyright file="PutFirmwareValidator.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Validators
{
    /// <summary>
    /// Validates the <see cref="PutFirmwareRequest"/> request body.
    /// </summary>
    public class PutFirmwareValidator : AbstractValidator<PutFirmwareRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutFirmwareValidator"/> class.
        /// </summary>
        public PutFirmwareValidator()
        {
            this.When(request => request.Package, () =>
            {
                this.RuleFor(request => request.Source)
                    .Null()
                    .OverridePropertyName("source");

                this.RuleFor(request => request.Upgrade)
                    .Null()
                    .OverridePropertyName("upgrade");
            });

            this.When(request => request.Source != null, () =>
            {
                this.RuleFor(request => request.Source)
                    .NotNull()
                    .NotEmpty()
                    .Must(source =>
                    {
                        var sources = new List<string>()
                        {
                            "stable",
                            "unstable",
                        };

                        return sources.Contains(source!);
                    })
                    .OverridePropertyName("source");
            });

            this.When(request => request.Upgrade != null, () =>
            {
                this.RuleFor(request => request.Upgrade)
                    .NotNull()
                    .NotEmpty()
                    .Must(source => source == "next")
                    .OverridePropertyName("upgrade");
            });
        }
    }
}
