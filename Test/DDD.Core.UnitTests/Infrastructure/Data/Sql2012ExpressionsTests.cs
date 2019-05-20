using FluentAssertions;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    public class Sql2012ExpressionsTests
    {
        #region Methods

        [Theory]
        [InlineData("sequence1", "schema1", "NEXT VALUE FOR schema1.sequence1")]
        [InlineData("sequence2", null, "NEXT VALUE FOR sequence2")]
        [InlineData("sequence3", "   ", "NEXT VALUE FOR sequence3")]
        [InlineData("sequence4", "   ", "NEXT VALUE FOR sequence4")]
        public void NextValue_WhenCalled_ReturnsEquivalentSqlExpression(string sequence, string schema, string expectedExpression)
        {
            // Arrange
            var expressions = new SqlServer2012Expressions();
            // Act
            var expression = expressions.NextValue(sequence, schema);
            // Assert
            expression.Should().BeEquivalentTo(expectedExpression);
        }

        #endregion Methods
    }
}