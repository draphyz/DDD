using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DDD.Core.Domain
{
    public class ValueObjectTests
    {

        #region Methods

        public static IEnumerable<object[]> ObjectsWithDifferentTypes()
        {
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new object()
            };
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1))
            };
        }

        public static IEnumerable<object[]> ValueObjectsAndPrimitiveEqualityComponents()
        {
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new object[] { "abcd", 1 }
            };
            yield return new object[]
            {
                new FakeComplexValueObject2("efjh", 1, new FakeComplexValueObject1("abcd", 2)),
                new object[] { "efjh", 1, "abcd", 2 }
            };
        }

        public static IEnumerable<object[]> ValueObjectsWithDifferentEqualityComponents()
        {
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject1("efjh", 1)
            };
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject1("abcd", 2)
            };
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject1("ABCD", 2)
            };
            yield return new object[]
            {
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1)),
                new FakeComplexValueObject2("efjh", 1, new FakeComplexValueObject1("abcd", 1))
            };
            yield return new object[]
            {
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1)),
                new FakeComplexValueObject2("abcd", 2, new FakeComplexValueObject1("abcd", 1))
            };
            yield return new object[]
            {
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1)),
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("efjh", 1))
            };
        }

        public static IEnumerable<object[]> ValueObjectsWithDifferentHashCodeComponents()

            => ValueObjectsWithDifferentEqualityComponents();

        public static IEnumerable<object[]> ValueObjectsWithDifferentTypes()
        {
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1))
            };
        }

        public static IEnumerable<object[]> ValueObjectsWithSameEqualityComponents()
        {
            yield return new object[]
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject1("abcd", 1)
            };
            yield return new object[]
            {
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1)),
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1))
            };
        }

        public static IEnumerable<object[]> ValueObjectsWithSameHashCodeComponents()

            => ValueObjectsWithSameEqualityComponents();

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentEqualityComponents))]
        public void Equals_ToObjectWithDifferentEqualityComponents_ReturnsFalse(object a, object b)
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
            object a = NewValueObject();
            object b = null;
            // Act 
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is null.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithSameEqualityComponents))]
        public void Equals_ToObjectWithSameEqualityComponents_ReturnsTrue(object a, object b)
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
            a = b = NewValueObject();
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument and current object refer to the same instance.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentEqualityComponents))]
        public void Equals_ToValueObjectWithDifferentEqualityComponents_ReturnsFalse(ValueObject a, ValueObject b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is non-null instance of the same type but semantically different from current object.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentTypes))]
        public void Equals_ToValueObjectWithDifferentType_ReturnsFalse(ValueObject a, ValueObject b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is non-null instance of a different type than the type of the current object.");
        }

        [Fact]
        public void Equals_ToValueObjectWithNullReference_ReturnsFalse()
        {
            // Arrange
            ValueObject a = NewValueObject();
            ValueObject b = null;
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeFalse("because argument is null.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithSameEqualityComponents))]
        public void Equals_ToValueObjectWithSameEqualityComponents_ReturnsTrue(ValueObject a, ValueObject b)
        {
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument is non-null instance of the same type and semantically equal to current object.");
        }

        [Fact]
        public void Equals_ToValueObjectWithSameReference_ReturnsTrue()
        {
            // Arrange
            ValueObject a, b;
            a = b = NewValueObject();
            // Act
            var result = a.Equals(b);
            // Assert
            result.Should().BeTrue("because argument and current object refer to the same instance");
        }

        [Fact]
        public void EqualToOperator_BothOperandsWithNullReference_ReturnsTrue()
        {
            // Arrange
            ValueObject a = null;
            ValueObject b = null;
            // Act
            var result = a == b;
            // Assert
            result.Should().BeTrue("because both operands are null.");
        }

        [Fact]
        public void EqualToOperator_OneOperandWithNullReference_ReturnsFalse()
        {
            // Arrange
            ValueObject a = NewValueObject();
            ValueObject b = null;
            // Act
            var result = a == b;
            // Assert
            result.Should().BeFalse("because one operand is null and the other one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentEqualityComponents))]
        public void EqualToOperator_OperandsWithDifferentEqualityComponents_ReturnsFalse(ValueObject a, ValueObject b)
        {
            // Act
            var result = a == b;
            // Assert
            result.Should().BeFalse("because operands are non-null instances of the same type but semantically different.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentTypes))]
        public void EqualToOperator_OperandsWithDifferentTypes_ReturnsFalse(ValueObject a, ValueObject b)
        {
            // Act
            var result = a == b;
            // Assert
            result.Should().BeFalse("because operands are non-null instances of a different type.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithSameEqualityComponents))]
        public void EqualToOperator_OperandsWithSameEqualityComponents_ReturnsTrue(ValueObject a, ValueObject b)
        {
            // Act
            var result = a == b;
            // Assert
            result.Should().BeTrue("operands are non-null instance of the same type and semantically equal.");
        }

        [Fact]
        public void EqualToOperator_OperandsWithSameReference_ReturnsTrue()
        {
            // Arrange
            ValueObject a, b;
            a = b = NewValueObject();
            // Act
            var result = a == b;
            // Assert
            result.Should().BeTrue("because operands refer to the same instance");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentHashCodeComponents))]
        public void GetHashCode_ValueObjectsWithDifferentHashCodeComponents_ReturnsDifferentValues(ValueObject a, ValueObject b)
        {
            // Act
            var hashCodeOfA = a.GetHashCode();
            var hashCodeOfB = b.GetHashCode();
            // Assert
            hashCodeOfA.Should().NotBe(hashCodeOfB, "because objects are semantically different.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithSameHashCodeComponents))]
        public void GetHashCode_ValueObjectsWithSameHashCodeComponents_ReturnsSameValue(ValueObject a, ValueObject b)
        {
            // Act
            var hashCodeOfA = a.GetHashCode();
            var hashCodeOfB = b.GetHashCode();
            // Assert
            hashCodeOfA.Should().Be(hashCodeOfB, "because objects are semantically equal.");
        }

        [Fact]
        public void NotEqualToOperator_BothOperandsWithNullReference_ReturnsFalse()
        {
            // Arrange
            ValueObject a = null;
            ValueObject b = null;
            // Act
            var result = a != b;
            // Assert
            result.Should().BeFalse("because both operands are null.");
        }

        [Fact]
        public void NotEqualToOperator_OneOperandWithNullReference_ReturnsTrue()
        {
            // Arrange
            ValueObject a = NewValueObject();
            ValueObject b = null;
            // Act
            var result = a != b;
            // Assert
            result.Should().BeTrue("because one operand is null and the other one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentEqualityComponents))]
        public void NotEqualToOperator_OperandsWithDifferentEqualityComponents_ReturnsTrue(ValueObject a, ValueObject b)
        {
            // Act
            var result = a != b;
            // Assert
            result.Should().BeTrue("because operands are non-null instances of the same type but semantically different.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithDifferentTypes))]
        public void NotEqualToOperator_OperandsWithDifferentTypes_ReturnsTrue(ValueObject a, ValueObject b)
        {
            // Act
            var result = a != b;
            // Assert
            result.Should().BeTrue("because operands are non-null instances of a different type.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsWithSameEqualityComponents))]
        public void NotEqualToOperator_OperandsWithSameEqualityComponents_ReturnsFalse(ValueObject a, ValueObject b)
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
            ValueObject a, b;
            a = b = NewValueObject();
            // Act
            var result = a != b;
            // Assert
            result.Should().BeFalse("because operands refer to the same instance.");
        }

        [Theory]
        [MemberData(nameof(ValueObjectsAndPrimitiveEqualityComponents))]
        public void PrimitiveEqualityComponents_WhenCalled_ReturnsExpectedResults(ValueObject valueObject, IEnumerable<object> expectedComponents)
        {
            // Act
            var components = valueObject.PrimitiveEqualityComponents();
            // Assert
            components.Should().ContainInOrder(expectedComponents);
        }

        private static ValueObject NewValueObject() => new FakeComplexValueObject1("abcd", 1);

        #endregion Methods

    }
}