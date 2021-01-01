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
        [InlineData(null, null, 1U)]
        [InlineData(null, null, uint.MaxValue)]
        [InlineData(null, uint.MinValue, null)]
        [InlineData(null, uint.MinValue, 1U)]
        [InlineData(null, uint.MinValue, uint.MaxValue)]
        [InlineData(null, uint.MaxValue, null)]
        [InlineData(null, uint.MaxValue, 1U)]
        [InlineData(null, uint.MaxValue, uint.MaxValue)]
        [InlineData("display_name", null, null)]
        [InlineData("display_name", null, 1U)]
        [InlineData("display_name", null, uint.MaxValue)]
        [InlineData("display_name", uint.MinValue, null)]
        [InlineData("display_name", uint.MinValue, 1U)]
        [InlineData("display_name", uint.MinValue, uint.MaxValue)]
        [InlineData("display_name", uint.MaxValue, null)]
        [InlineData("display_name", uint.MaxValue, 1U)]
        [InlineData("display_name", uint.MaxValue, uint.MaxValue)]
        public void ValidateSuccessTest(
            string? displayName,
            uint? frequncy,
            uint? requiredSensors)
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
        [InlineData(null, null, 0U)]
        public void ValidateFailureTest(
            string? displayName,
            uint? frequncy,
            uint? requiredSensors)
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
