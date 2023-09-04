// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;

/// <summary>
/// Wrapper attribute for specifying which customizations to load for a given test.
/// </summary>
public sealed class AutoDataWithCustomizationAttribute : AutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoDataWithCustomizationAttribute"/> class.
    /// </summary>
    /// <param name="customizations">The types of the customizations to load for the given test.</param>
    public AutoDataWithCustomizationAttribute(params Type[] customizations)
        : base(
            () =>
            {
                var composite = new CompositeCustomization(
                    customizations.Select(customization =>
                        CustomizationBuilder.CreateCustomization(customization)));

                var fixture = new Fixture().Customize(composite);

                fixture.Customize<DateOnly>(c => c.FromFactory<DateTime>(DateOnly.FromDateTime));

                return fixture;
            })
    {
    }
}
