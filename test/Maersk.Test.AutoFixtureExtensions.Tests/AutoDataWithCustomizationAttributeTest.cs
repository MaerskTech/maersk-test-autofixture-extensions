// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions.Tests;

using AutoFixture;
using FluentAssertions;
using Maersk.Test.AutoFixtureExtensions;
using Xunit;

[Trait(nameof(Category), Category.Unit)]
public sealed class AutoDataWithCustomizationAttributeTest
{
    public const string ExpectedStringValue = "sample";
    private static DateOnly _expectedDateOnly = new DateOnly(2023, 9, 4);

    [Trait(nameof(Category), Category.Unit)]
    public sealed class Constructor
    {
        [Fact]
        public void Given_valid_input_When_constructing_Then_it_creates_customization()
        {
            var result = new AutoDataWithCustomizationAttribute(typeof(SampleCustomization));

            var data = result.Should().NotBeNull();
        }

        [Theory]
        [AutoDataWithCustomization(typeof(SampleCustomization))]
        public void Given_a_sample_customization_with_the_AutoDataWithCustomization_attribute_When_testing_Then_it_uses_the_sample_attribute(string value)
        {
            value.Should().Be(ExpectedStringValue);
        }

        [Theory]
        [AutoDataWithCustomization(typeof(DateOnlyCustomization))]
        public void Given_a_DateOnly_value_When_testing_Then_it_can_generate_the_value(ClassWithDateOnly value)
        {
            value.DateOnly1.Should().Be(_expectedDateOnly);
        }
    }

    private class SampleCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => ExpectedStringValue);
        }
    }

    private class DateOnlyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var classWithDateOnly = fixture.Create<ClassWithDateOnly>();
            classWithDateOnly = classWithDateOnly with { DateOnly1 = _expectedDateOnly };

            fixture.Register(() => classWithDateOnly);
        }
    }
}
