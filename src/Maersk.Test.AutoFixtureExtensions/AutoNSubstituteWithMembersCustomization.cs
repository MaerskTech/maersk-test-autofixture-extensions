// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions;

using AutoFixture.AutoNSubstitute;

/// <summary>
/// Inherits from <seealso cref="AutoNSubstituteCustomization"/> and sets 'ConfigureMembers' to true.
/// </summary>
public class AutoNSubstituteWithMembersCustomization : AutoNSubstituteCustomization
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoNSubstituteWithMembersCustomization"/> class.
    /// Sets 'ConfigureMembers' to true.
    /// </summary>
    public AutoNSubstituteWithMembersCustomization()
    {
        ConfigureMembers = true;
    }
}
