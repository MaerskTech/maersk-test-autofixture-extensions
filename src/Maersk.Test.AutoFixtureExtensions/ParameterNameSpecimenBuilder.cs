// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using System;
using System.Reflection;
using AutoFixture.Kernel;

/// <summary>
/// Used to specify a value for a given parameter in the test method,
/// provided the name and type of that parameter.
/// </summary>
/// <typeparam name="T">The type of the parameter to set.</typeparam>
public class ParameterNameSpecimenBuilder<T> : ISpecimenBuilder
{
    private readonly string _name;
    private readonly T _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterNameSpecimenBuilder{T}/>"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter in the test method.</param>
    /// <param name="value">The value for the parameter in the test method.</param>
    /// <exception cref="ArgumentNullException">Thrown if a name for the parameter has not been provided.</exception>
    public ParameterNameSpecimenBuilder(string name, T value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _name = name;
        _value = value;
    }

    /// <summary>
    /// Creates a new specimen based on a request.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">A context that can be used to create other specimens.</param>
    /// <returns>The requested specimen if possible; otherwise a AutoFixture.Kernel.NoSpecimen instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is null.</exception>
    public object Create(object request, ISpecimenContext context)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!(request is ParameterInfo parameterInfo))
        {
            return new NoSpecimen();
        }

        if (parameterInfo.ParameterType != typeof(T) ||
            !string.Equals(
                parameterInfo.Name,
                _name,
                StringComparison.CurrentCultureIgnoreCase))
        {
            return new NoSpecimen();
        }

        if (_value is null)
        {
            throw new ArgumentException($"{nameof(_value)} not initialized.");
        }

        return _value;
    }
}
