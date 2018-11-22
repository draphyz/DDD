using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace DDD.Core.Domain
{
    [Trait("Category", "Unit")]
    public class DomainEntityTests
    {

        #region Methods

        public static IEnumerable<object[]> EntitiesWithDifferentIdentityComponents()
        {
            yield return new object[] 
            {
                new FakeSimpleEntity(new FakeIntIdentityComponent(1)),
                new FakeSimpleEntity(new FakeIntIdentityComponent(2))
            };
            yield return new object[] 
            {
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1)),
                new FakeComplexEntity(new FakeStringIdentityComponent("efjh"), new FakeIntIdentityComponent(1))
            };
            yield return new object[] 
            {
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1)),
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(2))
            };
            yield return new object[] 
            {
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1)),
                new FakeComplexEntity(new FakeStringIdentityComponent("ABCD"), new FakeIntIdentityComponent(1))
            };
        }

        public static IEnumerable<object[]> EntitiesWithDifferentTypes()
        {
            yield return new object[] 
            {
                new FakeSimpleEntity(new FakeIntIdentityComponent(1)),
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1))
            };
        }

        public static IEnumerable<object[]> EntitiesWithSameIdentityComponents()
        {
            yield return new object[] 
            {
                new FakeSimpleEntity(new FakeIntIdentityComponent(1)),
                new FakeSimpleEntity(new FakeIntIdentityComponent(1))
            };
            yield return new object[] 
            {
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1)),
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1))
            };
        }

        public static IEnumerable<object[]> ObjectsWithDifferentTypes()
        {
            yield return new object[] 
            {
                new FakeSimpleEntity(new FakeIntIdentityComponent(1)),
                new object()
            };
            yield return new object[] 
            {
                new FakeSimpleEntity(new FakeIntIdentityComponent(1)),
                new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1))
            };
        }

        [Theory]
        [MemberData(nameof(EntitiesWithDifferentIdentityComponents))]
        public void Equals_ToEntityWithDifferentIdentityComponents_ReturnsFalse(DomainEntity a, DomainEntity b)
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
        [MemberData(nameof(EntitiesWithSameIdentityComponents))]
        public void Equals_ToEntityWithSameIdentityComponents_ReturnsTrue(DomainEntity a, DomainEntity b)
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
        [MemberData(nameof(EntitiesWithDifferentIdentityComponents))]
        public void Equals_ToObjectWithDifferentIdentityComponents_ReturnsFalse(object a, object b)
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
        [MemberData(nameof(EntitiesWithSameIdentityComponents))]
        public void Equals_ToObjectWithSameIdentityComponents_ReturnsTrue(object a, object b)
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
        [MemberData(nameof(EntitiesWithDifferentIdentityComponents))]
        public void EqualToOperator_OperandsWithDifferentIdentityComponents_ReturnsFalse(DomainEntity a, DomainEntity b)
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
        [MemberData(nameof(EntitiesWithSameIdentityComponents))]
        public void EqualToOperator_OperandsWithSameIdentityComponents_ReturnsTrue(DomainEntity a, DomainEntity b)
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
        [MemberData(nameof(EntitiesWithDifferentIdentityComponents))]
        public void GetHashCode_EntitiesWithDifferentIdentityComponents_ReturnsDifferentValues(DomainEntity a, DomainEntity b)
        {
            // Act
            var hashCodeOfA = a.GetHashCode();
            var hashCodeOfB = b.GetHashCode();
            // Assert
            hashCodeOfA.Should().NotBe(hashCodeOfB, "because objects are semantically different.");
        }

        [Theory]
        [MemberData(nameof(EntitiesWithSameIdentityComponents))]
        public void GetHashCode_EntitiesWithSameIdentityComponents_ReturnsSameValue(DomainEntity a, DomainEntity b)
        {
            // Act
            var hashCodeOfA = a.GetHashCode();
            var hashCodeOfB = b.GetHashCode();
            // Assert
            hashCodeOfA.Should().Be(hashCodeOfB, "because objects are semantically equal.");
        }

        [Fact]
        public void IdentityAsString_ReturnsExpectedFormat()
        {
            // Arrange
            var entity = NewEntity();
            // Act
            var identityAsString = entity.IdentityAsString();
            // Assert
            identityAsString.Should().Be("abcd/1");
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
        [MemberData(nameof(EntitiesWithDifferentIdentityComponents))]
        public void NotEqualToOperator_OperandsWithDifferentIdentityComponents_ReturnsTrue(DomainEntity a, DomainEntity b)
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
        [MemberData(nameof(EntitiesWithSameIdentityComponents))]
        public void NotEqualToOperator_OperandsWithSameIdentityComponents_ReturnsFalse(DomainEntity a, DomainEntity b)
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

        private static DomainEntity NewEntity() => new FakeComplexEntity(new FakeStringIdentityComponent("abcd"), new FakeIntIdentityComponent(1));

        #endregion Methods
    }
}