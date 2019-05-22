using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace DDD.Core.Domain
{
    public class ComparableValueObjectTests
    {

        #region Methods

        public static IEnumerable<object[]> ComparableValueObjectsWithGreaterComparableComponents()
        {
            yield return new object[] 
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject1("bcde", 1)
            };
            yield return new object[] 
            {
                new FakeComplexValueObject1("abcd", 1),
                new FakeComplexValueObject1("abcd", 2)
            };
            yield return new object[] 
            {
                new FakeComplexValueObject1("abcd", 2),
                new FakeComplexValueObject1("bcde", 1)
            };
            yield return new object[] 
            {
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1)),
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("bcde", 1))
            };
        }

        public static IEnumerable<object[]> ComparableValueObjectsWithSameComparableComponents()
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

        public static IEnumerable<object[]> ComparableValueObjectsWithSmallerComparableComponents()
        {
            yield return new object[] 
            {
                new FakeComplexValueObject1("bcde", 1),
                new FakeComplexValueObject1("abcd", 1)
            };
            yield return new object[] 
            {
                new FakeComplexValueObject1("abcd", 2),
                new FakeComplexValueObject1("abcd", 1)
            };
            yield return new object[] 
            {
                new FakeComplexValueObject1("bcde", 1),
                new FakeComplexValueObject1("abcd", 2)
            };
            yield return new object[] 
            {
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("bcde", 1)),
                new FakeComplexValueObject2("abcd", 1, new FakeComplexValueObject1("abcd", 1))
            };
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        public void CompareTo_ComparableValueObjectWithGreaterComparableComponents_ReturnsLessThanZero(ComparableValueObject a, 
                                                                                                       ComparableValueObject b)
        {
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().BeNegative("because argument has greater comparable components than the current object.");
        }

        [Fact]
        public void CompareTo_ComparableValueObjectWithNullReference_ReturnsGreaterThanZero()
        {
            // Arrange
            ComparableValueObject a = NewComparableValueObject();
            ComparableValueObject b = null;
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().BePositive("because argument is null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void CompareTo_ComparableValueObjectWithSameComparableComponents_ReturnsZero(ComparableValueObject a, 
                                                                                            ComparableValueObject b)
        {
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().Be(0, "because argument has same comparable components than the current object.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void CompareTo_ComparableValueObjectWithSmallerComparableComponents_ReturnsGreaterThanZero(ComparableValueObject a, 
                                                                                                          ComparableValueObject b)
        {
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().BePositive("argument has smaller comparable components than the current object.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void CompareTo_ObjectsWithDifferentComparableComponents_ReturnInverseValues(ComparableValueObject a, 
                                                                                           ComparableValueObject b)
        {
            // Act
            var result1 = a.CompareTo(b);
            var result2 = -b.CompareTo(a);
            // Assert
            result1.Should().Be(result2, 
                "because if a.CompareTo(b) returns a value other than zero, then b.CompareTo(a) must return a value of the opposite sign.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void CompareTo_ObjectsWithSameComparableComponents_ReturnSameValue(IComparable a, IComparable b)
        {
            // Act
            var result1 = a.CompareTo(b);
            var result2 = b.CompareTo(a);
            // Assert
            result1.Should().Be(result2, "because if a.CompareTo(b) returns zero, then b.CompareTo(a) must also return zero.");
        }

        [Fact]
        public void CompareTo_ObjectWithDifferentType_ThrowsException()
        {
            // Arrange
            IComparable a = NewComparableValueObject();
            var b = new object();
            // Act
            Action compareTo = () => a.CompareTo(b);
            // Assert
            compareTo.Should().Throw<ArgumentException>("because argument is not the same type as the current object.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        public void CompareTo_ObjectWithGreaterComparableComponents_ReturnsLessThanZero(IComparable a, object b)
        {
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().BeNegative("because argument has greater comparable components than the current object.");
        }

        [Fact]
        public void CompareTo_ObjectWithNullReference_ThrowsException()
        {
            // Arrange
            IComparable a = NewComparableValueObject();
            object b = null;
            // Act
            Action compareTo = () => a.CompareTo(b);
            // Assert
            compareTo.Should().Throw<ArgumentException>("because argument is null");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void CompareTo_ObjectWithSameComparableComponents_ReturnsZero(IComparable a, object b)
        {
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().Be(0, "because argument has same comparable components than the current object.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void CompareTo_ObjectWithSmallerComparableComponents_ReturnsGreaterThanZero(IComparable a, object b)
        {
            // Act
            var result = a.CompareTo(b);
            // Assert
            result.Should().BePositive("because argument has smaller comparable components than the current object.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void CompareTo_PermuteComparableValueObjectsWithDifferentComparableComponents_ReturnsInverseValues(ComparableValueObject a, 
                                                                                                                  ComparableValueObject b)
        {
            // Act
            var result1 = a.CompareTo(b);
            var result2 = -b.CompareTo(a);
            // Assert
            result1.Should().Be(result2, 
                "because if a.CompareTo(b) returns a value other than zero, then b.CompareTo(a) must return a value of the opposite sign.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void CompareTo_PermuteComparableValueObjectsWithSameComparableComponents_ReturnsSameValue(ComparableValueObject a, 
                                                                                                         ComparableValueObject b)
        {
            // Act
            var result1 = a.CompareTo(b);
            var result2 = b.CompareTo(a);
            // Assert
            result1.Should().Be(result2, "because if a.CompareTo(b) returns zero, then b.CompareTo(a) must also return zero.");
        }

        [Fact]
        public void GreaterThanOperator_BothOperandsWithNullReference_ReturnsFalse()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = null;
            // Act
            var result = a > b;
            // Assert
            result.Should().BeFalse("because both operands are null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void GreaterThanOperator_BothOperandsWithSameComparableComponents_ReturnsFalse(ComparableValueObject a, 
                                                                                              ComparableValueObject b)
        {
            // Act
            var result = a > b;
            // Assert
            result.Should().BeFalse("because both operands have same comparable components.");
        }

        [Fact]
        public void GreaterThanOperator_FirstOperandWithNullReference_ReturnsFalse()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = NewComparableValueObject();
            // Act
            var result = a > b;
            // Assert
            result.Should().BeFalse("because the first operand is null and the second one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        public void GreaterThanOperator_SecondOperandWithGreaterComparableComponents_ReturnsFalse(ComparableValueObject a, 
                                                                                                  ComparableValueObject b)
        {
            // Act
            var result = a > b;
            // Assert
            result.Should().BeFalse("because the second operand has greater comparable components than the first one.");
        }

        [Fact]
        public void GreaterThanOperator_SecondOperandWithNullReference_ReturnsTrue()
        {
            // Arrange
            ComparableValueObject a = NewComparableValueObject();
            ComparableValueObject b = null;
            // Act
            var result = a > b;
            // Assert
            result.Should().BeTrue("because the second operand is null and the first one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void GreaterThanOperator_SecondOperandWithSmallerComparableComponents_ReturnsTrue(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a > b;
            // Assert
            result.Should().BeTrue("because the second operand has smaller comparable components than the first one.");
        }

        [Fact]
        public void GreaterThanOrEqualToOperator_BothOperandsWithNullReference_ReturnsTrue()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = null;
            // Act
            var result = a >= b;
            // Assert
            result.Should().BeTrue("because both operands are null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void GreaterThanOrEqualToOperator_BothOperandsWithSameComparableComponents_ReturnsTrue(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a >= b;
            // Assert
            result.Should().BeTrue("because both operands have same comparable components.");
        }

        [Fact]
        public void GreaterThanOrEqualToOperator_FirstOperandWithNullReference_ReturnsFalse()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = NewComparableValueObject();
            // Act
            var result = a >= b;
            // Assert
            result.Should().BeFalse("because the first operand is null and the second one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        public void GreaterThanOrEqualToOperator_SecondOperandWithGreaterComparableComponents_ReturnsFalse(ComparableValueObject a, 
                                                                                                           ComparableValueObject b)
        {
            // Act
            var result = a >= b;
            // Assert
            result.Should().BeFalse("because the second operand has greater comparable components than the first one.");
        }

        [Fact]
        public void GreaterThanOrEqualToOperator_SecondOperandWithNullReference_ReturnsTrue()
        {
            // Arrange
            ComparableValueObject a = NewComparableValueObject();
            ComparableValueObject b = null;
            // Act
            var result = a >= b;
            // Assert
            result.Should().BeTrue("because the second operand is null and the first one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void GreaterThanOrEqualToOperator_SecondOperandWithSmallerComparableComponents_ReturnsTrue(ComparableValueObject a, 
                                                                                                          ComparableValueObject b)
        {
            // Act
            var result = a >= b;
            // Assert
            result.Should().BeTrue("because the second operand has smaller comparable components than the first one.");
        }

        [Fact]
        public void LessThanOperator_BothOperandsWithNullReference_ReturnsFalse()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = null;
            // Act
            var result = a < b;
            // Assert
            result.Should().BeFalse("because both operands are null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void LessThanOperator_BothOperandsWithSameComparableComponents_ReturnsFalse(ComparableValueObject a, 
                                                                                           ComparableValueObject b)
        {
            // Act
            var result = a < b;
            // Assert
            result.Should().BeFalse("because both operands have same comparable components.");
        }

        [Fact]
        public void LessThanOperator_FirstOperandWithNullReference_ReturnsTrue()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = NewComparableValueObject();
            // Act
            var result = a < b;
            // Assert
            result.Should().BeTrue("because the first operand is null and the second one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        public void LessThanOperator_SecondOperandWithGreaterComparableComponents_ReturnsTrue(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a < b;
            // Assert
            result.Should().BeTrue("because the second operand has greater comparable components than the first one.");
        }

        [Fact]
        public void LessThanOperator_SecondOperandWithNullReference_ReturnsFalse()
        {
            // Arrange
            ComparableValueObject a = NewComparableValueObject();
            ComparableValueObject b = null;
            // Act
            var result = a < b;
            // Assert
            result.Should().BeFalse("because the second operand is null and the first one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void LessThanOperator_SecondOperandWithSmallerComparableComponents_ReturnsFalse(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a < b;
            // Assert
            result.Should().BeFalse("because the second operand has smaller comparable components than the first one.");
        }

        [Fact]
        public void LessThanOrEqualToOperator_BothOperandsWithNullReference_ReturnsTrue()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = null;
            // Act
            var result = a <= b;
            // Assert
            result.Should().BeTrue("because both operands are null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSameComparableComponents))]
        public void LessThanOrEqualToOperator_BothOperandsWithSameComparableComponents_ReturnsTrue(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a <= b;
            // Assert
            result.Should().BeTrue("because both operands have same comparable components.");
        }

        [Fact]
        public void LessThanOrEqualToOperator_FirstOperandWithNullReference_ReturnsTrue()
        {
            // Arrange
            ComparableValueObject a = null;
            ComparableValueObject b = NewComparableValueObject();
            // Act
            var result = a <= b;
            // Assert
            result.Should().BeTrue("because the first operand is null and the second one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithGreaterComparableComponents))]
        public void LessThanOrEqualToOperator_SecondOperandWithGreaterComparableComponents_ReturnsTrue(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a <= b;
            // Assert
            result.Should().BeTrue("because the second operand has greater comparable components than the first one.");
        }

        [Fact]
        public void LessThanOrEqualToOperator_SecondOperandWithNullReference_ReturnsFalse()
        {
            // Arrange
            ComparableValueObject a = NewComparableValueObject();
            ComparableValueObject b = null;
            // Act
            var result = a <= b;
            // Assert
            result.Should().BeFalse("because the second operand is null and the first one is non-null.");
        }

        [Theory]
        [MemberData(nameof(ComparableValueObjectsWithSmallerComparableComponents))]
        public void LessThanOrEqualToOperator_SecondOperandWithSmallerComparableComponents_ReturnsFalse(ComparableValueObject a, ComparableValueObject b)
        {
            // Act
            var result = a <= b;
            // Assert
            result.Should().BeFalse("because the second operand has smaller comparable components than the first one.");
        }

        private static ComparableValueObject NewComparableValueObject() => new FakeComplexValueObject1("abcd", 1);

        #endregion Methods
    }
}