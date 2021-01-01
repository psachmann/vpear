// <copyright file="PostDeviceValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PostDeviceValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PostDeviceRequest> validator;

        public PostDeviceValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PostDeviceRequest>>();
        }

        [Theory]
        [InlineData("192.192.10.0", "192.192.10.0")]
        [InlineData("192::0", "192::0")]
        [InlineData("ABC::0", "ABC::0")]
        public void ValidateSuccessTest(string? startIP, string? stopIP)
        {
            var request = new PostDeviceRequest()
            {
                Address = startIP,
                SubnetMask = stopIP,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("random", "random")]
        public void ValidateFailureTest(string? startIP, string? stopIP)
        {
            var request = new PostDeviceRequest()
            {
                Address = startIP,
                SubnetMask = stopIP,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
