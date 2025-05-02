using NUnit.Framework;
using DinoServer.Users;

namespace DinoServer.Tests.Users
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var user = new User("TestUser", 100);

            // Assert
            Assert.AreEqual("TestUser", user.Name);
            Assert.AreEqual(100, user.Score);
        }

        [Test]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var user = new User("TestUser", 100);

            // Act
            var result = user.ToString();

            // Assert
            Assert.AreEqual("TestUser, 100", result);
        }

        [Test]
        public void Equals_ShouldReturnTrueForEqualUsers()
        {
            // Arrange
            var user1 = new User("TestUser", 100);
            var user2 = new User("TestUser", 100);

            // Act & Assert
            Assert.IsTrue(user1.Equals(user2));
        }

        [Test]
        public void Equals_ShouldReturnFalseForDifferentUsers()
        {
            // Arrange
            var user1 = new User("TestUser", 100);
            var user2 = new User("AnotherUser", 200);

            // Act & Assert
            Assert.IsFalse(user1.Equals(user2));
        }

        [Test]
        public void GetHashCode_ShouldReturnSameHashForEqualUsers()
        {
            // Arrange
            var user1 = new User("TestUser", 100);
            var user2 = new User("TestUser", 100);

            // Act
            var hash1 = user1.GetHashCode();
            var hash2 = user2.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_ShouldReturnDifferentHashForDifferentUsers()
        {
            // Arrange
            var user1 = new User("TestUser", 100);
            var user2 = new User("AnotherUser", 200);

            // Act
            var hash1 = user1.GetHashCode();
            var hash2 = user2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
