using System;
using System.IO;
using Xunit;
using FluentAssertions;

namespace DDD.Core.Infrastructure.Serialization
{
    public class JsonSerializerWrapperTests : IDisposable
    {

        #region Fields

        private readonly Stream stream = new MemoryStream();

        #endregion Fields

        #region Methods

        public void Dispose()
        {
            this.stream.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void SerializeAndDeserialize_AreSymmetric()
        {
            // Arrange
            var obj1 = new FakePerson
            {
                FirstName = "Donald",
                LastName = "Duck",
                Birthdate = new DateTime(1934, 4, 29)
            };
            var serializer = JsonSerializerWrapper.Create();
            // Act
            serializer.Serialize(this.stream, obj1);
            this.stream.Position = 0;
            var obj2 = serializer.Deserialize<FakePerson>(this.stream);
            // Assert
            obj2.Should().BeEquivalentTo(obj1);
        }

        #endregion Methods

    }
}
