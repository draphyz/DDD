using FluentAssertions;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    public class DbStandardExpressionsTests
    {

        #region Methods

        [Fact]
        public void FromDummy_WhenCalled_ReturnsEquivalentSqlExpression()
        {
            // Arrange
            var expressions = new FakeDbStandardExpressions();
            // Act
            var expression = expressions.FromDummy();
            // Assert
            expression.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public void ParameterPrefix_WhenCalled_ReturnsEquivalentSqlExpression()
        {
            // Arrange
            var expressions = new FakeDbStandardExpressions();
            // Act
            var expression = expressions.ParameterPrefix();
            // Assert
            expression.Should().BeEquivalentTo("@");
        }

        #endregion Methods

    }
}