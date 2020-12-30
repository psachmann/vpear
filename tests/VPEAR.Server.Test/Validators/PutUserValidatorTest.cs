// <copyright file="PutUserValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PutUserValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PutUserRequest> validator;

        public PutUserValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutUserRequest>>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("old_password", "new_password")]
        public void ValidateSuccessTest(
            string? newPassword,
            string? oldPassword)
        {
            var request = new PutUserRequest()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("password", "")]
        [InlineData("", "password")]
        [InlineData(null, "password")]
        [InlineData("password", null)]
        [InlineData("password", "password")]
        [InlineData("short", "long_enough")]
        [InlineData("long_enough", "short")]
        public void ValidateFailureTest(
            string? newPassword,
            string? oldPassword)
        {
            var request = new PutUserRequest()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
