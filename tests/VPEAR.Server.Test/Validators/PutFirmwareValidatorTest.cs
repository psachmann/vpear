// <copyright file="PutFirmwareValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PutFirmwareValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PutFirmwareRequest> validator;

        public PutFirmwareValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutFirmwareRequest>>();
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData("stable", null, false)]
        [InlineData("unstable", null, false)]
        [InlineData("stable", "next", false)]
        [InlineData("unstable", "next", false)]
        [InlineData(null, "next", false)]
        [InlineData(null, null, true)]
        public void ValidateSuccessTest(
            string? source,
            string? upgrade,
            bool package)
        {
            var request = new PutFirmwareRequest()
            {
                Package = package,
                Source = source,
                Upgrade = upgrade,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData("stable", null, true)]
        [InlineData("unstable", null, true)]
        [InlineData(null, "next", true)]
        [InlineData("", null, true)]
        [InlineData("", "", true)]
        [InlineData(null, "", true)]
        [InlineData("", null, false)]
        [InlineData("", "", false)]
        [InlineData(null, "", false)]
        public void ValidateFailureTest(
            string? source,
            string? upgrade,
            bool package)
        {
            var request = new PutFirmwareRequest()
            {
                Package = package,
                Source = source,
                Upgrade = upgrade,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
