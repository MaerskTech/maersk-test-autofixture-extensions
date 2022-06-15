# Introduction
This package contains a number of useful extension methods for [AutoFixture](https://github.com/AutoFixture/AutoFixture), making working with AutoFixture simpler.

# Examples
## Simple scenario
For basic usage you can instrument your unit test method using the `AutoDataWithCustomization` attribute like this:
```csharp
[Theory]
[AutoDataWithCustomization(typeof(AutoMoqCustomization))]
public void Given_some_input_When_mapping_Then_it_maps(
    Vessel vessel,
    VesselMapper sut)
{
    // ...
}
```

### ConfigureMembers = `true`

If you need to set [ConfigureMembers](https://github.com/AutoFixture/AutoFixture/blob/f58f618046281335ae88b7471248b8eba5b65cc8/Src/AutoMoq/AutoMoqCustomization.cs#L48) to `true`, then do like this instead (this will instruct AutoFixture that members of a mock will be automatically setup to retrieve the return values from the fixture):
```csharp
[Theory]
[AutoDataWithCustomization(typeof(AutoMoqWithMembersCustomization))]
public void Given_some_input_When_mapping_Then_it_maps(
    Vessel vessel,
    VesselMapper sut)
{
    // ...
}
```

## Providing a customization
A common pattern is to move some of the test customization to a separate class. You can do that like this:

```csharp
[Theory]
[AutoDataWithCustomization(typeof(AutoMoqCustomization), typeof(VesselMapperCustomization))]
public void Given_some_input_When_mapping_Then_it_maps(
    Vessel vessel,
    VesselMapper sut)
{
    // ...
}

private class VesselMapperCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        // ...
    }
}
```

The code placed in the `VesselMapperCustomization` will get executed before the test.

## Provide inline parameters to customization
It can be useful to pass inline parameters to the customization and to the test method - this allows re-using the same test in multiple scenarios.
You can do this using the `InlineAutoDataWithCustomization` attribute:
```csharp
[Theory]
[InlineAutoDataWithCustomization(typeof(VesselMapperCustomization), "vessel code 1", "vessel name 1")]
[InlineAutoDataWithCustomization(typeof(VesselMapperCustomization), "vessel code 2", "vessel name 2")]
[InlineAutoDataWithCustomization(typeof(VesselMapperCustomization), "vessel code 3", "vessel name 3")]
public void Given_some_input_and_a_specific_vessel_code_When_mapping_Then_it_maps(
    string vesselCode,
    string vesselName,
    Vessel vessel,
    VesselMapper sut)
{
    // ...
}

private class VesselMapperCustomization : CompositeCustomization
{
    public VesselMapperCustomization(
        string vesselCode,
        string vesselName)
        : base(new AutoMoqCustomization { ConfigureMembers = true }, new AutoMoqWithMembersCustomization())
    {
        // ...
    }
}
```
`vessel code 1` will get assigned to `string vesselCode`, because it is the first parameter to match the type (`string`).
Likewise, `vessel name 1` will get assigned to `string vesselName`, because it is the second parameter to match the type (`string`).
It is of course possible to use different types.

Note that the customization is inheriting from `CompositeCustomization`, and providing some additional values for that types constructor. 

### Only pass parameters to the customization
If you don't need to pass the inline parameters to the test method, then use the `InlineCustomizationData` attribute.

## Specifying a value for a test parameter
The `ParameterNameSpecimenBuilder` can be used to set a value for a specific parameter in the test method:

```csharp
[Theory]
[AutoDataWithCustomization(typeof(AutoMoqCustomization), typeof(VesselMapperCustomization))]
public void Given_some_input_When_mapping_Then_it_maps(
    DateTime scheduleArrivalTimeUtc,
    Vessel vessel,
    VesselMapper sut)
{
    // ...
}

private class VesselMapperCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        DateTime scheduleArrivalTimeUtc = // ...

        fixture.Customizations.Add(new ParameterNameSpecimenBuilder<DateTimeOffset>(nameof(scheduleArrivalTimeUtc), scheduleArrivalTimeUtc));
    }
}
```


# Contribute
Read the [CONTRIBUTING.md](./CONTRIBUTING.md) file.

# Changes

# Credits
This library was originally written by:
- [Mikael Kühn](https://github.com/mkumaersk)
- [Nicolai Sørensen](https://github.com/nicolai-sorensen1-maersk)
- [Sebastian Dittmann](https://github.com/sebastiankdittmann)
- [Oleksandr Ostapenko](https://github.com/oos018)

Unit tests and documentation added by:
- [Christian Guldbaek](https://github.com/christian-guldbaek)
