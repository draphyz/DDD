using FluentAssertions;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    public class Oracle11ExpressionsTests
    {

        #region Methods

        [Fact]
        public void FromDummy_WhenCalled_ReturnsEquivalentSqlExpression()
        {
            // Arrange
            var expressions = new Oracle11Expressions();
            // Act
            var expression = expressions.FromDummy();
            // Assert
            expression.Should().BeEquivalentTo("FROM DUAL");
        }

        [Theory]
        [InlineData("sequence1", "schema1", "schema1.sequence1.NEXTVAL")]
        [InlineData("sequence2", null, "sequence2.NEXTVAL")]
        [InlineData("sequence3", "   ", "sequence3.NEXTVAL")]
        [InlineData("sequence4", "   ", "sequence4.NEXTVAL")]
        public void NextValue_WhenCalled_ReturnsEquivalentSqlExpression(string sequence, string schema, string expectedExpression)
        {
            // Arrange
            var expressions = new Oracle11Expressions();
            // Act
            var expression = expressions.NextValue(sequence, schema);
            // Assert
            expression.Should().BeEquivalentTo(expectedExpression);
        }

        [Fact]
        public void ParameterPrefix_WhenCalled_ReturnsEquivalentSqlExpression()
        {
            // Arrange
            var expressions = new Oracle11Expressions();
            // Act
            var expression = expressions.ParameterPrefix();
            // Assert
            expression.Should().BeEquivalentTo(":");
        }

        #endregion Methods

    }
}