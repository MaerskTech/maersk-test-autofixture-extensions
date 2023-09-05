// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions.Tests;

using System;
using System.Drawing;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Maersk.Test.AutoFixtureExtensions;
using Xunit;

[Trait(nameof(Category), Category.Unit)]
public sealed class InlineAutoDataWithCustomizationAttributeTest
{
    [Trait(nameof(Category), Category.Unit)]
    public sealed class Constructor
    {
        private const string ExpectedArgument1 = "ExpectedArgument1";
        private const double ExpectedArgument2 = 123.4;
        private const int ExpectedDateOnlyYear = 2023;

        [Theory]
        [AutoData]
        public void Given_null_inlineDataCustomization_When_instantiating_with_3_arguments_Then_throws_ArgumentNullException(string[] arguments)
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataWithCustomizationAttribute(
                null,
                new Type[] { typeof(SampleCustomizationWithArguments) },
                arguments));
        }

        [Theory]
        [AutoData]
        public void Given_null_otherCustomizations_When_instantiating_with_3_arguments_Then_throws_ArgumentNullException(string[] arguments)
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataWithCustomizationAttribute(
                typeof(SampleCustomizationWithArguments),
                null,
                arguments));
        }

        [Fact]
        public void Given_null_arguments_When_instantiating_with_3_arguments_Then_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataWithCustomizationAttribute(
                typeof(SampleCustomizationWithArguments),
                new Type[] { typeof(SampleCustomizationWithArguments) },
                null));
        }

        [Theory]
        [AutoData]
        public void Given_null_inlineDataCustomization_When_instantiating_with_2_arguments_Then_throws_ArgumentNullException(string[] arguments)
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataWithCustomizationAttribute(null, arguments));
        }

        [Fact]
        public void Given_null_arguments_When_instantiating_with_2_arguments_Then_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataWithCustomizationAttribute(
                typeof(SampleCustomizationWithArguments),
                null));
        }

        [Theory]
        [AutoData]
        public void Given_an_invalid_customization_type_for_inlineDataCustomization_with_3_arguments_When_creating_Then_it_throws_ArgumentException(string[] arguments)
        {
            var otherCustomizations = new Type[] { typeof(SampleCustomizationWithArguments) };

            Assert.Throws<ArgumentException>(() => new InlineAutoDataWithCustomizationAttribute(
                typeof(Rectangle),
                otherCustomizations,
                arguments));
        }

        [Theory]
        [AutoData]
        public void Given_an_invalid_customization_type_for_otherCustomizations_with_3_arguments_When_creating_Then_it_throws_ArgumentException(string[] arguments)
        {
            var otherCustomizations = new Type[]
            {
                typeof(SampleCustomizationWithArguments),
                typeof(Rectangle),
            };

            Assert.Throws<ArgumentException>(() => new InlineAutoDataWithCustomizationAttribute(
                typeof(SampleCustomizationWithArguments),
                otherCustomizations,
                arguments));
        }

        [Theory]
        [AutoData]
        public void Given_an_invalid_customization_type_for_inlineDataCustomization_with_2_arguments_When_creating_Then_it_throws_ArgumentException(string[] arguments)
        {
            Assert.Throws<ArgumentException>(() => new InlineAutoDataWithCustomizationAttribute(
                typeof(Rectangle),
                arguments));
        }

        [Theory]
        [InlineAutoDataWithCustomization(
            typeof(SampleCustomizationWithArguments),
            ExpectedArgument1,
            ExpectedArgument2)]
        public void Given_arguments_with_only_inline_customizations_When_creating_Then_arguments_are_transferred_to_the_test_method(
            string argument1,
            double argument2)
        {
            argument1.Should().Be(ExpectedArgument1);
            argument2.Should().Be(ExpectedArgument2);
        }

        [Theory]
        [InlineAutoDataWithCustomization(
            typeof(SampleCustomizationWithArgumentsAndVerification),
            ExpectedArgument1,
            ExpectedArgument2)]
        public void Given_arguments_with_only_inline_customizations_When_creating_Then_arguments_are_transferred_to_customization(
            string argument1,
            double argument2,
            SampleCustomizationWithArgumentsAndVerification customization)
        {
            argument1.Should().Be(ExpectedArgument1);
            argument2.Should().Be(ExpectedArgument2);

            customization.Argument1.Should().Be(ExpectedArgument1);
            customization.Argument2.Should().Be(ExpectedArgument2);
        }

        [Theory]
        [InlineAutoDataWithCustomization(
            typeof(SampleCustomizationWithArguments),
            new Type[] { typeof(OtherSampleCustomizationWithArguments) },
            ExpectedArgument1,
            ExpectedArgument2)]
        public void Given_arguments_with_other_customizations_When_creating_Then_arguments_are_transferred_to_the_test_method(
            string argument1,
            double argument2)
        {
            argument1.Should().Be(ExpectedArgument1);
            argument2.Should().Be(ExpectedArgument2);
        }

        [Theory]
        [InlineAutoDataWithCustomization(
            typeof(SampleCustomizationWithDateOnlyArguments),
            new Type[] { typeof(OtherSampleCustomizationWithDateOnly) },
            ExpectedDateOnlyYear)]
        public void Given_arguments_with_DateOnly_inline_customizations_When_creating_Then_arguments_are_transferred_to_the_test_method(
            int year)
        {
            year.Should().Be(ExpectedDateOnlyYear);
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

        private class OtherSampleCustomizationWithArguments : ICustomization
        {
            public OtherSampleCustomizationWithArguments()
            {
            }

            public void Customize(IFixture fixture)
            {
            }
        }

        private class OtherSampleCustomizationWithDateOnly : ICustomization
        {
            public OtherSampleCustomizationWithDateOnly()
            {
            }

            public void Customize(IFixture fixture)
            {
                _ = fixture.Create<ClassWithDateOnly>();
            }
        }

        private class SampleCustomizationWithDateOnlyArguments : ICustomization
        {
            public SampleCustomizationWithDateOnlyArguments(int year)
            {
                _ = year;
            }

            public void Customize(IFixture fixture)
            {
            }
        }
    }
}
