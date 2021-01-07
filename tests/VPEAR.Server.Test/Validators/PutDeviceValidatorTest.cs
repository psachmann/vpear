// <copyright file="PutDeviceValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PutDeviceValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PutDeviceRequest> validator;

        public PutDeviceValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutDeviceRequest>>();
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, null, 1)]
        [InlineData(null, null, int.MaxValue)]
        [InlineData(null, 1, null)]
        [InlineData(null, 1, 1)]
        [InlineData(null, 1, int.MaxValue)]
        [InlineData(null, int.MaxValue, null)]
        [InlineData(null, int.MaxValue, 1)]
        [InlineData(null, int.MaxValue, int.MaxValue)]
        [InlineData("display_name", null, null)]
        [InlineData("display_name", null, 1)]
        [InlineData("display_name", null, int.MaxValue)]
        [InlineData("display_name", 1, null)]
        [InlineData("display_name", 1, 1)]
        [InlineData("display_name", 1, int.MaxValue)]
        [InlineData("display_name", int.MaxValue, null)]
        [InlineData("display_name", int.MaxValue, 1)]
        [InlineData("display_name", int.MaxValue, int.MaxValue)]
        public void ValidateSuccessTest(
            string? displayName,
            int? frequncy,
            int? requiredSensors)
        {
            var request = new PutDeviceRequest()
            {
                DisplayName = displayName,
                Frequency = frequncy,
                RequiredSensors = requiredSensors,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData("", null, null)]
        [InlineData(null, null, 0)]
        [InlineData(null, 0, null)]
        [InlineData(null, 0, 0)]
        [InlineData("", 0, 0)]
        public void ValidateFailureTest(
            string? displayName,
            int? frequncy,
            int? requiredSensors)
        {
            var request = new PutDeviceRequest()
            {
                DisplayName = displayName,
                Frequency = frequncy,
                RequiredSensors = requiredSensors,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
