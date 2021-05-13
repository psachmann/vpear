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
        private readonly IValidator<PutPasswordRequest> validator;

        public PutUserValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutPasswordRequest>>();
        }

        [Theory]
        [InlineData("old_password", "new_password", "token")]
        public void ValidateSuccessTest(
            string newPassword,
            string oldPassword,
            string token)
        {
            var request = new PutPasswordRequest()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
                Token = token,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("password", "", "")]
        [InlineData("", "password", "")]
        [InlineData("", "", "token")]
        [InlineData(null, "password", null)]
        [InlineData("password", null, null)]
        [InlineData("password", "password", "token")]
        [InlineData("short", "long_enough", "token")]
        [InlineData("long_enough", "short", "token")]
        public void ValidateFailureTest(
            string? newPassword,
            string? oldPassword,
            string? token)
        {
            var request = new PutPasswordRequest()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
                Token = token,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
