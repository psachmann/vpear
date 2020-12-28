// <copyright file="PostRegisterValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PostRegisterValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PostRegisterRequest> validator;

        public PostRegisterValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PostRegisterRequest>>();
        }

        [Theory]
        [InlineData(null, "example@domain.tld", "password")]
        [InlineData("display_name", "example@domain.tld", "password")]
        public void ValidateSuccessTest(
            string? displayName,
            string email,
            string password)
        {
            var request = new PostRegisterRequest()
            {
                DisplayName = displayName,
                Email = email,
                Password = password,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "", "")]
        [InlineData(null, "example@domain.tld", "")]
        [InlineData("", "example@domain.tld", "password")]
        [InlineData(null, "example@domain.tld", "pass")]
        public void ValidateFailureTest(
            string? displayName,
            string email,
            string password)
        {
            var request = new PostRegisterRequest()
            {
                DisplayName = displayName,
                Email = email,
                Password = password,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
