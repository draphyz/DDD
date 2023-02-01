using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DDD.Linq
{
    public class IEnumerableExtensionsTests
    {

        #region Methods

        public static IEnumerable<object[]> SkipLastMemberData()
        {
            yield return new object[]
            {
                new [] {1, 2, 3, 4, 5 },
                2,
                new[] {1, 2, 3 }
            };
            yield return new object[]
            {
                new [] {1, 2, 3, 4, 5 },
                1,
                new[] {1, 2, 3, 4 }
            };
            yield return new object[]
            {
                new [] {1, 2, 3, 4, 5 },
                -5,
                new[] {1, 2, 3, 4, 5 }
            };
        }

        [Theory]
        [MemberData(nameof(SkipLastMemberData))]
        public void SkipLast_WhenCalled_ReturnsExpectedResult(IEnumerable<int> source, int count, IEnumerable<int> expected)
        {
            // Act
            var result = IEnumerableExtensions.SkipLast(source, count);
            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        #endregion Methods

    }
}
