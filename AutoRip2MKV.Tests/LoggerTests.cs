using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoRip2MKV.Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void Logger_Debug_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.Debug("Test debug message");
            }
            catch (Exception)
            {
                Assert.Fail("Logger.Debug should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_Info_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.Info("Test info message");
            }
            catch (Exception)
            {
                Assert.Fail("Logger.Info should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_Warn_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.Warn("Test warning message");
            }
            catch (Exception)
            {
                Assert.Fail("Logger.Warn should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_Error_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.Error("Test error message");
            }
            catch (Exception)
            {
                Assert.Fail("Logger.Error should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_LogOperationStart_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.LogOperationStart("TestOperation", "param1", "param2");
            }
            catch (Exception)
            {
                Assert.Fail("Logger.LogOperationStart should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_LogOperationComplete_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.LogOperationComplete("TestOperation", TimeSpan.FromSeconds(1));
            }
            catch (Exception)
            {
                Assert.Fail("Logger.LogOperationComplete should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_LogOperationFailure_DoesNotThrow()
        {
            // Arrange
            var testException = new InvalidOperationException("Test exception");

            // Act & Assert
            try
            {
                Logger.LogOperationFailure("TestOperation", testException);
            }
            catch (Exception)
            {
                Assert.Fail("Logger.LogOperationFailure should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_InfoWithParameters_DoesNotThrow()
        {
            // Arrange & Act & Assert
            try
            {
                Logger.Info("Test message with parameters: {0}, {1}", "param1", 42);
            }
            catch (Exception)
            {
                Assert.Fail("Logger.Info with parameters should not throw an exception");
            }
        }

        [TestMethod]
        public void Logger_ErrorWithException_DoesNotThrow()
        {
            // Arrange
            var testException = new InvalidOperationException("Test exception");

            // Act & Assert
            try
            {
                Logger.Error(testException, "Error message with exception");
            }
            catch (Exception)
            {
                Assert.Fail("Logger.Error with exception should not throw an exception");
            }
        }
    }
}
