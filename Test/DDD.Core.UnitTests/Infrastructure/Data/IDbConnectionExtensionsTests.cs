using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Trait("Category", "Unit")]
    public class IDbConnectionExtensionsTests
    {
        #region Methods

        public static IEnumerable<object[]> Connections()
        {
            yield return new object[]
            {
                NewConnection("System.Data.SqlClient"), typeof(SqlServer2012Expressions)
            };
            yield return new object[]
            {
                NewConnection("Oracle.ManagedDataAccess.Client"), typeof(Oracle11Expressions)
            };
        }

        [Theory]
        [MemberData(nameof(Connections))]
        public void StandardExpressions_WhenCalled_ReturnsExpectedExpressions(IDbConnection connection, Type expectedExpressionsType)
        {
            // Act
            var expressions = connection.Expressions();
            // Assert
            expressions.Should().BeOfType(expectedExpressionsType);
        }

        private static IDbConnection NewConnection(string providerInvariantName)
        {
            var providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
            return providerFactory.CreateConnection();
        }

        #endregion Methods
    }
}