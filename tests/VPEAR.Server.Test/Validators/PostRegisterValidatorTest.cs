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
        [InlineData("user_name", "password")]
        public void ValidateSuccessTest(string? name, string? password)
        {
            var request = new PostRegisterRequest()
            {
                Name = name,
                Password = password,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("example@email.tld", null)]
        [InlineData(null, "password")]
        [InlineData("example@email.tld", "")]
        [InlineData("", "password")]
        [InlineData("example@domain.tld", "short")]
        public void ValidateFailureTest(string name, string password)
        {
            var request = new PostRegisterRequest()
            {
                Name = name,
                Password = password,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
