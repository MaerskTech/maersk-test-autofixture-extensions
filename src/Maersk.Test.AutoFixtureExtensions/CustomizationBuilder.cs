// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using System;
using AutoFixture;

/// <summary>
/// Helper class for creating instances for customization types.
/// </summary>
public static class CustomizationBuilder
{
    /// <summary>
    /// Creates an instance of the provided customization type.
    /// </summary>
    /// <param name="customization">The customization type, must implement <seealso cref="ICustomization"/>.</param>
    /// <returns>An instance of the provided customization type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if null input.</exception>
    /// <exception cref="ArgumentException">Thrown if an instance could not be created of if the type doesn't implement ICustomization.</exception>
    public static ICustomization CreateCustomization(Type customization)
    {
        if (customization is null)
        {
            throw new ArgumentNullException(nameof(customization));
        }

        if (!typeof(ICustomization).IsAssignableFrom(customization))
        {
            throw new ArgumentException("The customization type must implement ICustomization", nameof(customization));
        }

        var instance = Activator.CreateInstance(customization);

        if (instance is null)
        {
            throw new ArgumentException(nameof(customization));
        }

        return (ICustomization)instance;
    }

    /// <summary>
    /// Creates an instance of the provided customization type, and allows passing in arguments to the construction.
    /// </summary>
    /// <param name="customization">The customization type, must implement <seealso cref="ICustomization"/>.</param>
    /// <param name="arguments">Arguments to use when using the Activator to create the instance.</param>
    /// <returns>An instance of the provided customization type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if null input.</exception>
    /// <exception cref="ArgumentException">Thrown if an instance could not be created of if the type doesn't implement ICustomization.</exception>
    public static ICustomization CreateCustomizationWithArguments(
        Type customization,
        object[] arguments)
    {
        if (customization is null)
        {
            throw new ArgumentNullException(nameof(customization));
        }

        if (!typeof(ICustomization).IsAssignableFrom(customization))
        {
            throw new ArgumentException("The customization type must implement ICustomization", nameof(customization));
        }

        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        var instance = Activator.CreateInstance(customization, arguments);

        if (instance is null)
        {
            throw new ArgumentException();
        }

        return (ICustomization)instance;
    }
}
