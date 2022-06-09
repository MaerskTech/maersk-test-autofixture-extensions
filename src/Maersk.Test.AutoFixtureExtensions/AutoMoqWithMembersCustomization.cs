// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using AutoFixture.AutoMoq;

/// <summary>
/// Inherits from <seealso cref="AutoMoqCustomization"/> and sets 'ConfigureMembers' to true.
/// </summary>
public class AutoMoqWithMembersCustomization : AutoMoqCustomization
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMoqWithMembersCustomization"/> class.
    /// Sets 'ConfigureMembers' to true.
    /// </summary>
    public AutoMoqWithMembersCustomization()
    {
        ConfigureMembers = true;
    }
}
