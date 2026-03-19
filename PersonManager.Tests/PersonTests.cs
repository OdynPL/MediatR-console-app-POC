using PersonManager.Domain;
namespace PersonManager.Tests
{
    public class PersonTests
    {
        [Fact]
        public void Person_CreatedWithName_IsNotNull()
        {
            // Arrange
            var person = new Person { Name = "Adam" };

            // Act & Assert
            Assert.NotNull(person);
            Assert.Equal("Adam", person.Name);
        }
    }
}