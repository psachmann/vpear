// <copyright file="PutWifiValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PutWifiValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PutWifiRequest> validator;

        public PutWifiValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutWifiRequest>>();
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("ssid", "password", null)]
        [InlineData(null, null, "direct")]
        [InlineData(null, null, "indirect")]
        [InlineData("ssid", "password", "direct")]
        [InlineData("ssid", "password", "indirect")]
        public void ValidateSuccessTest(
            string? ssid,
            string? password,
            string? mode)
        {
            var request = new PutWifiRequest()
            {
                Mode = mode,
                Password = password,
                Ssid = ssid,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "Validation should be successful.");
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("", "", null)]
        [InlineData(null, null, "")]
        [InlineData(null, null, "random")]
        public void ValidateFailureTest(
            string? ssid,
            string? password,
            string? mode)
        {
            var request = new PutWifiRequest()
            {
                Mode = mode,
                Password = password,
                Ssid = ssid,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "Validation should be unsuccessful.");
        }
    }
}
