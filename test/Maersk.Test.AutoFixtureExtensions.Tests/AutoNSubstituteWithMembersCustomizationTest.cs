// Copyright (c) Maersk. All rights reserved.
// Licensed under the Apache License. See LICENSE in the project root for license information.

namespace Maersk.Test.AutoFixtureExtensions.Tests;

using FluentAssertions;
using Maersk.Test.AutoFixtureExtensions;
using Xunit;

[Trait(nameof(Category), Category.Unit)]
public sealed class AutoNSubstituteWithMembersCustomizationTest
{
    [Trait(nameof(Category), Category.Unit)]
    public sealed class Constructor
    {
        [Fact]
        public void Given_no_input_When_creating_an_instance_Then_it_sets_ConfigureMembers_true()
        {
            var result = new AutoNSubstituteWithMembersCustomization();

            result.ConfigureMembers.Should().BeTrue();
        }
    }
}
