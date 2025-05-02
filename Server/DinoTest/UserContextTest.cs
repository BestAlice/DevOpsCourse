using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using DinoServer.Users;

namespace DinoServer.Tests.Users
{
    [TestFixture]
    public class UserContextTests
    {
        private DbContextOptions<UserContext> _options;
        private UserContext _userContext;

        [SetUp]
        public void Setup()
        {
            // Use an in-memory database for testing
            _options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _userContext = new UserContext(_options);
        }

        [TearDown]
        public void TearDown()
        {
            _userContext.Database.EnsureDeleted();
            _userContext.Dispose();
        }

        [Test]
        public void AddBook_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User", Score = 100 };

            // Act
            _userContext.AddBook(user);

            // Assert
            var users = _userContext.Users.ToList();
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(user.Id, users[0].Id);
            Assert.AreEqual(user.Name, users[0].Name);
            Assert.AreEqual(user.Score, users[0].Score);
        }

        [Test]
        public async Task AddUserAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User", Score = 100 };

            // Act
            var result = await _userContext.AddUserAsync(user);

            // Assert
            var users = await _userContext.Users.ToListAsync();
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(user.Id, users[0].Id);
            Assert.AreEqual(user.Name, users[0].Name);
            Assert.AreEqual(user.Score, users[0].Score);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.Score, result.Score);
        }
    }
}
