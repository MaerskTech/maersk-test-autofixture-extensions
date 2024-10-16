// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;

/// <summary>
/// Pushes arguments into the customization constructor, and to the test method.
/// If you do NOT need to push arguments to the test method, please use <seealso cref="InlineCustomizationDataAttribute"/>.
/// </summary>
public sealed class InlineAutoDataWithCustomizationAttribute : InlineAutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InlineAutoDataWithCustomizationAttribute"/> class.
    /// </summary>
    /// <param name="inlineDataCustomization">The type of the customization to push parameters into.</param>
    /// <param name="otherCustomizations">Additional types to push parameters into.</param>
    /// <param name="arguments">The arguments to push into the customization constructors.</param>
    public InlineAutoDataWithCustomizationAttribute(
        Type inlineDataCustomization,
        Type[] otherCustomizations,
        params object[] arguments)
        : base(
            new AutoDataWithFactoryAttribute(() =>
            {
                var compositeCustomization = new CompositeCustomization(
                    CreateCompositeCustomization(
                        inlineDataCustomization,
                        otherCustomizations,
                        arguments));

                var fixture = new Fixture();

                fixture.Customize<DateOnly>(c => c.FromFactory<DateTime>(DateOnly.FromDateTime));

                fixture.Customize(compositeCustomization);

                return fixture;
            }),
            arguments)
    {
        if (inlineDataCustomization is null)
        {
            throw new ArgumentNullException(nameof(inlineDataCustomization));
        }

        if (otherCustomizations is null)
        {
            throw new ArgumentNullException(nameof(otherCustomizations));
        }

        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        if (!typeof(ICustomization).IsAssignableFrom(inlineDataCustomization))
        {
            throw new ArgumentException("The inlineDataCustomization type must implement ICustomization", nameof(inlineDataCustomization));
        }

        if (otherCustomizations.Any(customization => !typeof(ICustomization).IsAssignableFrom(customization)))
        {
            throw new ArgumentException("The otherCustomizations type must implement ICustomization", nameof(inlineDataCustomization));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineAutoDataWithCustomizationAttribute"/> class.
    /// </summary>
    /// <param name="inlineDataCustomization">The type of the customization to push parameters into.</param>
    /// <param name="arguments">The arguments to push into the customization constructor.</param>
    public InlineAutoDataWithCustomizationAttribute(
        Type inlineDataCustomization,
        params object[] arguments)
        : base(
            new AutoDataWithFactoryAttribute(() =>
            {
                return new Fixture().Customize(
                    CustomizationBuilder.CreateCustomizationWithArguments(
                        inlineDataCustomization,
                        arguments));
            }),
            arguments)
    {
        if (inlineDataCustomization is null)
        {
            throw new ArgumentNullException(nameof(inlineDataCustomization));
        }

        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        if (!typeof(ICustomization).IsAssignableFrom(inlineDataCustomization))
        {
            throw new ArgumentException("The inlineDataCustomization type must implement ICustomization", nameof(inlineDataCustomization));
        }
    }

    private static IEnumerable<ICustomization> CreateCompositeCustomization(
        Type inlineDataCustomization,
        Type[] otherCustomizations,
        object[] arguments)
    {
        var compositeCustomization = otherCustomizations.Select(
            customization => CustomizationBuilder.CreateCustomization(customization))
                        .Append(CustomizationBuilder.CreateCustomizationWithArguments(
                            inlineDataCustomization,
                            arguments));

        return compositeCustomization;
    }
}
