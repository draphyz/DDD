using Xunit;
using System.Collections.Generic;
using FluentAssertions;

namespace DDD.Common.Domain
{
    public class FlagsEnumerationTests
    {

        #region Methods

        public static IEnumerable<object[]> ValidCodesAndResults()
        {
            yield return new object[] { "FFK1, FFK3", FakeFlagsEnumeration.Create(5, "FFK1, FFK3", "Fake1, Fake3") };
            yield return new object[] { "FFK3, FFK1", FakeFlagsEnumeration.Create(5, "FFK1, FFK3", "Fake1, Fake3") };
            yield return new object[] { "FFK1, FFK2, FFK3", FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
            yield return new object[] { "FFK2, FFK1, FFK3", FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
            yield return new object[] { "FFK2, FFK3, FFK1", FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
        }

        public static IEnumerable<object[]> ValidNamesAndResults()
        {
            yield return new object[] { "Fake1, Fake3", FakeFlagsEnumeration.Create(5, "FFK1, FFK3", "Fake1, Fake3") };
            yield return new object[] { "Fake3, Fake1", FakeFlagsEnumeration.Create(5, "FFK1, FFK3", "Fake1, Fake3") };
            yield return new object[] { "Fake1, Fake2, Fake3", FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
            yield return new object[] { "Fake2, Fake1, Fake3", FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
            yield return new object[] { "Fake2, Fake3, Fake1", FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
        }

        public static IEnumerable<object[]> ValidValuesAndResults()
        {
            yield return new object[] { 5, FakeFlagsEnumeration.Create(5, "FFK1, FFK3", "Fake1, Fake3") };
            yield return new object[] { 7, FakeFlagsEnumeration.Create(7, "FFK1, FFK2, FFK3", "Fake1, Fake2, Fake3") };
        }
        [Theory]
        [MemberData(nameof(ValidCodesAndResults))]
        public void ParseCode_WithValidCodes_ReturnsExpectedResult(string code, FakeFlagsEnumeration expectedResult)
        {
            // Act 
            var result = FlagsEnumeration.ParseCode<FakeFlagsEnumeration>(code);
            // Assert
            result.Should().BeEquivalentTo(expectedResult, o => o.ComparingByMembers<FakeFlagsEnumeration>());
        }

        [Theory]
        [MemberData(nameof(ValidNamesAndResults))]
        public void ParseName_WithValidNames_ReturnsExpectedResult(string name, FakeFlagsEnumeration expectedResult)
        {
            // Act 
            var result = FlagsEnumeration.ParseName<FakeFlagsEnumeration>(name);
            // Assert
            result.Should().BeEquivalentTo(expectedResult, o => o.ComparingByMembers<FakeFlagsEnumeration>());
        }

        [Theory]
        [MemberData(nameof(ValidValuesAndResults))]
        public void ParseValue_WithValidValues_ReturnsExpectedResult(int value, FakeFlagsEnumeration expectedResult)
        {
            // Act 
            var result = FlagsEnumeration.ParseValue<FakeFlagsEnumeration>(value);
            // Assert
            result.Should().BeEquivalentTo(expectedResult, o => o.ComparingByMembers<FakeFlagsEnumeration>());
        }

        [Theory]
        [MemberData(nameof(ValidCodesAndResults))]
        public void TryParseCode_WithValidCodes_SetsExpectedResult(string code, FakeFlagsEnumeration expectedResult)
        {
            // Act 
            FlagsEnumeration.TryParseCode<FakeFlagsEnumeration>(code, false, out var result);
            // Assert
            result.Should().BeEquivalentTo(expectedResult, o => o.ComparingByMembers<FakeFlagsEnumeration>());
        }

        [Theory]
        [MemberData(nameof(ValidNamesAndResults))]
        public void TryParseName_WithValidNames_SetsExpectedResult(string name, FakeFlagsEnumeration expectedResult)
        {
            // Act 
            FlagsEnumeration.TryParseName<FakeFlagsEnumeration>(name, false, out var result);
            // Assert
            result.Should().BeEquivalentTo(expectedResult, o => o.ComparingByMembers<FakeFlagsEnumeration>());
        }

        [Theory]
        [MemberData(nameof(ValidValuesAndResults))]
        public void TryParseValue_WithValidValues_SetsExpectedResult(int value, FakeFlagsEnumeration expectedResult)
        {
            // Act 
            FlagsEnumeration.TryParseValue<FakeFlagsEnumeration>(value, out var result);
            // Assert
            result.Should().BeEquivalentTo(expectedResult, o => o.ComparingByMembers<FakeFlagsEnumeration>());
        }

        #endregion Methods
    }
}
