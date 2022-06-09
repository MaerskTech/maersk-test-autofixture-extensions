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
public sealed class CustomizationBuilderTest
{
    [Trait(nameof(Category), Category.Unit)]
    public sealed class CreateCustomization
    {
        [Fact]
        public void Given_null_input_When_creating_Then_it_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CustomizationBuilder.CreateCustomization(null));
        }

        [Fact]
        public void Given_an_invalid_customization_type_When_creating_Then_it_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => CustomizationBuilder.CreateCustomization(typeof(Rectangle)));
        }

        [Fact]
        public void Given_a_valid_customization_type_When_creating_Then_it_creates_an_instance()
        {
            var result = CustomizationBuilder.CreateCustomization(typeof(SampleCustomization));

            result.Should().NotBeNull();
            result.GetType().Should().Be(typeof(SampleCustomization));
        }

        private class SampleCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
            }
        }
    }

    [Trait(nameof(Category), Category.Unit)]
    public sealed class CreateCustomizationWithArguments
    {
        [Theory]
        [AutoData]
        public void Given_null_type_When_creating_Then_it_throws_ArgumentNullException(string[] arguments)
        {
            Assert.Throws<ArgumentNullException>(() => CustomizationBuilder.CreateCustomizationWithArguments(null, arguments));
        }

        [Fact]
        public void Given_null_arguments_When_creating_Then_it_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CustomizationBuilder.CreateCustomizationWithArguments(typeof(SampleCustomizationWithArguments), null));
        }

        [Theory]
        [AutoData]
        public void Given_an_invalid_customization_type_When_creating_Then_it_throws_ArgumentException(string[] arguments)
        {
            Assert.Throws<ArgumentException>(() => CustomizationBuilder.CreateCustomizationWithArguments(typeof(Rectangle), arguments));
        }

        [Theory]
        [AutoData]
        public void Given_a_valid_customization_type_When_creating_Then_it_creates_an_instance(string argument1, double argument2)
        {
            object[] arguments = new object[] { argument1, argument2 };
            var result = CustomizationBuilder.CreateCustomizationWithArguments(typeof(SampleCustomizationWithArguments), arguments);

            result.Should().NotBeNull();
            result.GetType().Should().Be(typeof(SampleCustomizationWithArguments));
        }

        [Theory]
        [AutoData]
        public void Given_a_valid_customization_type_When_creating_Then_it_creates_an_instance_with_arguments_set(string argument1, double argument2)
        {
            object[] arguments = new object[] { argument1, argument2 };
            var sample = (SampleCustomizationWithArguments)CustomizationBuilder.CreateCustomizationWithArguments(typeof(SampleCustomizationWithArguments), arguments);

            sample.Argument1.Should().Be(argument1);
            sample.Argument2.Should().Be(argument2);
        }

        private class SampleCustomizationWithArguments : ICustomization
        {
            public SampleCustomizationWithArguments(string argument1, double argument2)
            {
                Argument1 = argument1;
                Argument2 = argument2;
            }

            public string Argument1 { get; }

            public double Argument2 { get; }

            public void Customize(IFixture fixture)
            {
            }
        }
    }
}
