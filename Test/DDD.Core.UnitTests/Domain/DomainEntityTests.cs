using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DDD.Core.Domain
{
    public class DomainEntityTests
    {

        #region Methods

        public static IEnumerable<object[]> EntitiesAndIdentityAsString()
        {
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                "1"
            };
            yield return new object[]
            {
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1)),
                "abcd/1"
            };
            yield return new object[]
            {
                new FakeEntity4(new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("efgh", 2))),
                "abcd/1/efgh/2"
            };
        }

        public static IEnumerable<object[]> EntitiesWithDifferentIdentities()
        {
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new FakeEntity1(new FakeSimpleValueObject1(2))
            };
            yield return new object[]
            {
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1)),
                new FakeEntity2(new FakeComplexValueObject1("efjh", 1))
            };
            yield return new object[]
            {
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1)),
                new FakeEntity2(new FakeComplexValueObject1("abcd", 2))
            };
            yield return new object[]
            {
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1)),
                new FakeEntity2(new FakeComplexValueObject1("ABCD", 1))
            };
        }

        public static IEnumerable<object[]> EntitiesWithDifferentTypes()
        {
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new FakeEntity3(new FakeSimpleValueObject1(1))
            };
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1))
            };
        }

        public static IEnumerable<object[]> EntitiesWithSameIdentity()
        {
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new FakeEntity1(new FakeSimpleValueObject1(1))
            };
            yield return new object[]
            {
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1)),
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1))
            };
        }

        public static IEnumerable<object[]> ObjectsWithDifferentTypes()
        {
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new object()
            };
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new FakeEntity2(new FakeComplexValueObject1("abcd", 1))
            };
            yield return new object[]
            {
                new FakeEntity1(new FakeSimpleValueObject1(1)),
                new FakeEntity3(new FakeSimpleValueObject1(1))
            };
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentIdentities))]
        public void Equals_ToEntityWithDifferentIdentities_ReturnsFalse(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is non-null instance of the same type but semantically different from current object.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentTypes))]
        public void Equals_ToEntityWithDifferentType_ReturnsFalse(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is non-null instance of a different type than the type of the current object.");
        }

        [Fact]
        public void Equals_ToEntityWithNullReference_ReturnsFalse()
        {
            // Arrange
            DomainEntity a = NewEntity();
            DomainEntity b = null;
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is null.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithSameIdentity))]
        public void Equals_ToEntityWithSameIdentity_ReturnsTrue(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument is non-null instance of the same type and semantically equal to current object.");
        }

        [Fact]
        public void Equals_ToEntityWithSameReference_ReturnsTrue()
        {
            // Arrange
            DomainEntity a, b;
            a = b = NewEntity();
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument and current object refer to the same instance.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentIdentities))]
        public void Equals_ToObjectWithDifferentIdentities_ReturnsFalse(object a, object b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is non-null instance of the same type but semantically different from current object.");
        }

        [Theory]
        [MemberData(nameof(ObjectsWithDifferentTypes))]
        public void Equals_ToObjectWithDifferentType_ReturnsFalse(object a, object b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is non-null instance of a different type than the type of the current object.");
        }

        [Fact]
        public void Equals_ToObjectWithNullReference_ReturnsFalse()
        {
            // Arrange
            object a = NewEntity();
            object b = null;
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is null.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithSameIdentity))]
        public void Equals_ToObjectWithSameIdentity_ReturnsTrue(object a, object b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument is non-null instance of the same type and semantically equal to current object.");
        }

        [Fact]
        public void Equals_ToObjectWithSameReference_ReturnsTrue()
        {
            // Arrange
            object a, b;
            a = b = NewEntity();
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument and current object refer to the same instance.");
        }

        [Fact]
        public void EqualToOperator_BothOperandsWithNullReference_ReturnsTrue()
        {
            // Arrange
            DomainEntity a = null;
            DomainEntity b = null;
            // Act
            var result = a == b;
            // Assert
            result.Should().BeTrue("because both operands are null.");
        }

        [Fact]
        public void EqualToOperator_OneOperandWithNullReference_ReturnsFalse()
        {
            // Arrange
            DomainEntity a = NewEntity();
            DomainEntity b = null;
            // Act
            var result = a == b;
            // Assert
            result.Should().BeFalse("one operand is null and the other one is non-null.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentIdentities))]
        public void EqualToOperator_OperandsWithDifferentIdentities_ReturnsFalse(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a == b;
            // Assert
            result.Should().BeFalse("because operands are non-null instances of the same type but semantically different.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentTypes))]
        public void EqualToOperator_OperandsWithDifferentTypes_ReturnsFalse(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a == b;
            // Assert
            result.Should().BeFalse("because operands are non-null instances of a different type.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithSameIdentity))]
        public void EqualToOperator_OperandsWithSameIdentity_ReturnsTrue(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a == b;
            // Assert
            result.Should().BeTrue("because operands are non-null instance of the same type and semantically equal.");
        }

        [Fact]
        public void EqualToOperator_OperandsWithSameReference_ReturnsTrue()
        {
            // Arrange
            DomainEntity a, b;
            a = b = NewEntity();
            // Act
            var result = a == b;
            // Assert
            result.Should().BeTrue("because operands refer to the same instance.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentIdentities))]
        public void GetHashCode_EntitiesWithDifferentIdentities_ReturnsDifferentValues(DomainEntity a, DomainEntity b)
        {
            // Act
            var hashCodeOfA = a.GetHashCode();
            var hashCodeOfB = b.GetHashCode();
            // Assert
            hashCodeOfA.Should().NotBe(hashCodeOfB, "because objects are semantically different.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithSameIdentity))]
        public void GetHashCode_EntitiesWithSameIdentity_ReturnsSameValue(DomainEntity a, DomainEntity b)
        {
            // Act
            var hashCodeOfA = a.GetHashCode();
            var hashCodeOfB = b.GetHashCode();
            // Assert
            hashCodeOfA.Should().Be(hashCodeOfB, "because objects are semantically equal.");
        }

        [Theory]
        [MemberData(nameof(EntitiesAndIdentityAsString))]
        public void IdentityAsString_ReturnsExpectedValue(DomainEntity entity, string expectedIdentity)
        {
            // Act
            var identityAsString = entity.IdentityAsString();
            // Assert
            identityAsString.Should().Be(expectedIdentity);
        }

        [Fact]
        public void NotEqualToOperator_BothOperandsWithNullReference_ReturnsFalse()
        {
            // Arrange
            DomainEntity a = null;
            DomainEntity b = null;
            // Act
            var result = a != b;
            // Assert
            result.Should().BeFalse("because both operands are null.");
        }

        [Fact]
        public void NotEqualToOperator_OneOperandWithNullReference_ReturnsTrue()
        {
            // Arrange
            DomainEntity a = NewEntity();
            DomainEntity b = null;
            // Act
            var result = a != b;
            // Assert
            result.Should().BeTrue("because one operand is null and the other one is non-null.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentIdentities))]
        public void NotEqualToOperator_OperandsWithDifferentIdentities_ReturnsTrue(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a != b;
            // Assert
            result.Should().BeTrue("because operands are non-null instances of the same type but semantically different.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentTypes))]
        public void NotEqualToOperator_OperandsWithDifferentTypes_ReturnsTrue(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a != b;
            // Assert
            result.Should().BeTrue("because operands are non-null instances of a different type.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithSameIdentity))]
        public void NotEqualToOperator_OperandsWithSameIdentity_ReturnsFalse(DomainEntity a, DomainEntity b)
        {
            // Act
            var result = a != b;
            // Assert
            result.Should().BeFalse("because operands are non-null instance of the same type and semantically equal.");
        }

        [Fact]
        public void NotEqualToOperator_OperandsWithSameReference_ReturnsFalse()
        {
            // Arrange
            DomainEntity a, b;
            a = b = NewEntity();
            // Act
            var result = a != b;
            // Assert
            result.Should().BeFalse("because operands refer to the same instance");
        }

        private static DomainEntity NewEntity() => new FakeEntity2(new FakeComplexValueObject1("abcd", 1));

        #endregion Methods

    }
}