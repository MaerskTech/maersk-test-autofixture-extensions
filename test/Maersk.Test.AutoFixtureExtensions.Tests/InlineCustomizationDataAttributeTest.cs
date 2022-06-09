// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions.Tests;

using System.Drawing;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Maersk.Test.AutoFixtureExtensions;
using Xunit;

[Trait(nameof(Category), Category.Unit)]
public sealed class InlineCustomizationDataAttributeTest
{
    [Trait(nameof(Category), Category.Unit)]
    public sealed class Constructor
    {
        private const string ExpectedArgument1 = "ExpectedArgument1";
        private const double ExpectedArgument2 = 123.4;

        [Theory]
        [AutoData]
        public void Given_null_inlineDataCustomization_When_instantiating_Then_throws_ArgumentNullException(string[] arguments)
        {
            Assert.Throws<ArgumentNullException>(() => new InlineCustomizationDataAttribute(
                null,
                arguments));
        }

        [Fact]
        public void Given_null_arguments_When_instantiating_Then_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineCustomizationDataAttribute(
                typeof(SampleCustomizationWithArguments),
                null));
        }

        [Fact]
        public void Given_an_invalid_customization_type_for_inlineDataCustomization_When_creating_Then_it_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new InlineCustomizationDataAttribute(
                typeof(Rectangle),
                null));
        }

        [Theory]
        [InlineCustomizationData(
            typeof(SampleCustomizationWithArguments),
            ExpectedArgument1,
            ExpectedArgument2)]
        public void Given_arguments_with_only_inline_customizations_When_creating_Then_arguments_are_not_transferred_to_the_test_method(
            string argument1,
            double argument2)
        {
            argument1.Should().NotBe(ExpectedArgument1);
            argument2.Should().NotBe(ExpectedArgument2);
        }

        [Theory]
        [InlineCustomizationData(
            typeof(SampleCustomizationWithArgumentsAndVerification),
            ExpectedArgument1,
            ExpectedArgument2)]
        public void Given_arguments_with_only_inline_customizations_When_creating_Then_arguments_are_transferred_to_customization(
            string argument1,
            double argument2,
            SampleCustomizationWithArgumentsAndVerification customization)
        {
            argument1.Should().NotBe(ExpectedArgument1);
            argument2.Should().NotBe(ExpectedArgument2);

            customization.Argument1.Should().Be(ExpectedArgument1);
            customization.Argument2.Should().Be(ExpectedArgument2);
        }

        public class SampleCustomizationWithArgumentsAndVerification : ICustomization
        {
            public SampleCustomizationWithArgumentsAndVerification(string argument1, double argument2)
            {
                Argument1 = argument1;
                Argument2 = argument2;
            }

            public string Argument1 { get; }

            public double Argument2 { get; }

            public void Customize(IFixture fixture)
            {
                fixture.Register(() => this);
            }
        }

        private class SampleCustomizationWithArguments : ICustomization
        {
            public SampleCustomizationWithArguments(string argument1, double argument2)
            {
            }

            public void Customize(IFixture fixture)
            {
            }
        }
    }
}
