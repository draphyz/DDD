using FluentAssertions;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    public class SequentialSqlServerGuidGeneratorTests
    {

        #region Methods

        [Fact]
        public void Generate_WhenCalledConsecutively_GeneratesSequentialSqlServerValues()
        {
            // Arrange
            var generator = new SequentialSqlServerGuidGenerator();
            var sqlServerValues = new List<SqlGuid>();
            // Act
            for (var i = 0; i < 100; i++)
                sqlServerValues.Add(new SqlGuid(generator.Generate()));
            // Assert
            sqlServerValues.Should().BeInAscendingOrder();
        }

        #endregion Methods

    }
}