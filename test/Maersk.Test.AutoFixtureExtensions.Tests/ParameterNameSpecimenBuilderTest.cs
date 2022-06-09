// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions.Tests;

using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Maersk.Test.AutoFixtureExtensions;
using Moq;
using Xunit;

[Trait(nameof(Category), Category.Unit)]
public sealed class ParameterNameSpecimenBuilderTest
{
    [Trait(nameof(Category), Category.Unit)]
    public sealed class Constructor
    {
        [Fact]
        public void TestGuards()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(typeof(ParameterNameSpecimenBuilder<string>).GetConstructors());
        }
    }

    [Trait(nameof(Category), Category.Unit)]
    public sealed class Create
    {
        private const string ExpectedStringValue = "sample";

        [Theory]
        [AutoData]
        public void Given_null_request_When_creating_Then_it_throws_ArgumentNullException(
            string name,
            string value,
            Mock<ISpecimenContext> context)
        {
            var builder = new ParameterNameSpecimenBuilder<string>(name, value);

            Assert.Throws<ArgumentNullException>(() => builder.Create(null, context.Object));
        }

        [Theory]
        [AutoData]
        public void Given_null_context_When_creating_Then_it_throws_ArgumentNullException(
            string name,
            string value,
            string randomObject)
        {
            var builder = new ParameterNameSpecimenBuilder<string>(name, value);

            Assert.Throws<ArgumentNullException>(() => builder.Create(randomObject, null));
        }

        [Theory]
        [AutoData]
        public void Given_incorrect_request_type_When_creating_Then_it_returns_no_specimen(
           string name,
           string value,
           string randomObject,
           Mock<ISpecimenContext> context)
        {
            var builder = new ParameterNameSpecimenBuilder<string>(name, value);

            var specimen = builder.Create(randomObject, context.Object);

            specimen.Should().BeOfType(typeof(NoSpecimen));
        }

        [Theory]
        [AutoDataWithCustomization(typeof(SampleCustomization))]
        public void Given_valid_configuration_When_using_the_ParameterNameSpecimenBuilder_in_a_customization_Then_it_injects_the_expected_value(
            string randomValue,
            string expectedValue)
        {
            expectedValue.Should().Be(ExpectedStringValue);

            randomValue.Should().NotBe(ExpectedStringValue);
        }

        private class SampleCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customizations.Add(new ParameterNameSpecimenBuilder<string>(
                    "expectedValue",
                    ExpectedStringValue));
            }
        }
    }
}
