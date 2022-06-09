// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using System;
using AutoFixture;
using AutoFixture.Xunit2;

/// <summary>
/// Pushes arguments into the customization constructor, but does not push it to the test method.
/// If you need to push arguments to the test method, please use <seealso cref="InlineAutoDataWithCustomizationAttribute"/>.
/// </summary>
public sealed class InlineCustomizationDataAttribute : InlineAutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InlineCustomizationDataAttribute"/> class.
    /// </summary>
    /// <param name="inlineDataCustomization">The type of the customization to push arguments into.</param>
    /// <param name="arguments">The arguments to push into the customization.</param>
    public InlineCustomizationDataAttribute(
        Type inlineDataCustomization,
        params object[] arguments)
        : base(
            new AutoDataWithFactoryAttribute(() => new Fixture().Customize(
                    Activator.CreateInstance(
                        inlineDataCustomization,
                        arguments) as ICustomization)))
    {
        if (inlineDataCustomization is null)
        {
            throw new ArgumentNullException(nameof(inlineDataCustomization));
        }

        if (!typeof(ICustomization).IsAssignableFrom(inlineDataCustomization))
        {
            throw new ArgumentException("The inlineDataCustomization type must implement ICustomization", nameof(inlineDataCustomization));
        }

        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }
    }
}
