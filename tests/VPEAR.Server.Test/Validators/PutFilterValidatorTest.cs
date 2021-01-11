// <copyright file="PutFilterValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PutFilterValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PutFilterRequest> validator;

        public PutFilterValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutFilterRequest>>();
        }

        [Fact]
        public void ValidateSuccessTest()
        {
            var request = new PutFilterRequest();
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "Allways valid.");
        }
    }
}
