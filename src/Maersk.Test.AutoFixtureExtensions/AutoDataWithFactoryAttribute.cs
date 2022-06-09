// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using System;
using AutoFixture;
using AutoFixture.Xunit2;

/// <summary>
/// Inherits from <seealso cref="AutoDataAttribute"/> and passes in a factory for the fixture.
/// </summary>
public class AutoDataWithFactoryAttribute : AutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoDataWithFactoryAttribute"/> class.
    /// </summary>
    /// <param name="fixtureFactory">A factory method to use when creating the fixture.</param>
    public AutoDataWithFactoryAttribute(Func<IFixture> fixtureFactory)
        : base(fixtureFactory)
    {
    }
}
