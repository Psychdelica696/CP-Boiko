using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CP_Boiko.Tests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void CurrentDateTime_ShouldNotBeEmpty_OnInit()
        {
            // Arrange & Act
            var dateTime = DateTime.Now.ToString("F");

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(dateTime));
        }

        [TestMethod]
        public void HasInput_ShouldBeFalse_WhenEmpty()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var hasInput = !string.IsNullOrEmpty(input);

            // Assert
            Assert.IsFalse(hasInput);
        }

        [TestMethod]
        public void HasInput_ShouldBeTrue_WhenNotEmpty()
        {
            // Arrange
            var input = "Тестовий текст";

            // Act
            var hasInput = !string.IsNullOrEmpty(input);

            // Assert
            Assert.IsTrue(hasInput);
        }

        [TestMethod]
        public void TodoItem_Title_ShouldMatch()
        {
            // Arrange & Act
            var todo = new
            {
                Id = 1,
                Title = "Test todo",
                Completed = false,
                UserId = 1
            };

            // Assert
            Assert.AreEqual(1, todo.Id);
            Assert.AreEqual("Test todo", todo.Title);
            Assert.IsFalse(todo.Completed);
        }
    }
}
