using FluentAssertions;
using Xunit;

namespace DDD.Common.Domain
{
    public class FullNameTests
    {
        #region Methods

        [Fact]
        public void AsFormattedName_WhenCalled_ReturnsExpectedFormat()
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            var formattedName = fullName.AsFormattedName();
            // Assert
            formattedName.Should().Be("DUPONT Paul");
        }

        [Theory]
        [InlineData("jacques")]
        [InlineData("JACQUES")]
        [InlineData("JacQUes")]
        public void Constructor_WhenCalled_InitializesFirstNameWithExpectedCase(string firstName)
        {
            // Act
            var fullName = new FullName("Dupont", firstName);
            // Assert
            fullName.FirstName.Should().Be("Jacques");
        }

        [Theory]
        [InlineData("dupont")]
        [InlineData("DUPONT")]
        [InlineData("DuPOnt")]
        public void Constructor_WhenCalled_InitializesLastNameWithExpectedCase(string lastName)
        {
            // Act
            var fullName = new FullName(lastName, "Jacques");
            // Assert
            fullName.LastName.Should().Be("Dupont");
        }

        [Theory]
        [InlineData("jacques")]
        [InlineData("JACQUES")]
        [InlineData("JacQUes")]
        public void WithFirstName_WhenCalled_ReturnsFullNameWithExpectedFirstName(string newFirstName)
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            var newFullName = fullName.WithFirstName(newFirstName);
            // Assert
            newFullName.FirstName.Should().Be("Jacques");
        }

        [Fact]
        public void WithFirstName_WhenCalled_ReturnsNewInstance()
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            var newFullName = fullName.WithFirstName("Jacques");
            // Assert
            newFullName.Should().NotBeSameAs(fullName);
        }

        [Fact]
        public void WithFirstName_WhenCalled_UnchangesFirstNameOfCurrentInstance()
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            fullName.WithFirstName("Jacques");
            // Assert
            fullName.FirstName.Should().Be("Paul");
        }

        [Theory]
        [InlineData("dubois")]
        [InlineData("DUBOIS")]
        [InlineData("DuBOis")]
        public void WithLastName_WhenCalled_ReturnsFullNameWithExpectedLastName(string newLastName)
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            var newFullName = fullName.WithLastName(newLastName);
            // Assert
            newFullName.LastName.Should().Be("Dubois");
        }

        [Fact]
        public void WithLastName_WhenCalled_ReturnsNewInstance()
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            var newFullName = fullName.WithLastName("Dubois");
            // Assert
            newFullName.Should().NotBeSameAs(fullName);
        }

        [Fact]
        public void WithLastName_WhenCalled_UnchangesLastNameOfCurrentInstance()
        {
            // Arrange
            var fullName = new FullName("Dupont", "Paul");
            // Act
            fullName.WithLastName("Dubois");
            // Assert
            fullName.LastName.Should().Be("Dupont");
        }

        #endregion Methods
    }
}