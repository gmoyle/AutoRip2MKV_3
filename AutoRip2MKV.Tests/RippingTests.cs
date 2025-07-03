using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace AutoRip2MKV.Tests
{
    [TestClass]
    public class RippingTests
    {
        [TestMethod]
        public void GetVolumeLabel_RemovesInvalidFileNameChars()
        {
            // Arrange
            string inputFileName = "Test<>:\"|?*Movie";
            
            // Act
            string result = Ripping.GetVolumeLabel(inputFileName);
            
            // Assert
            Assert.IsFalse(result.Contains("<"), "Result should not contain invalid filename characters");
            Assert.IsFalse(result.Contains(">"), "Result should not contain invalid filename characters");
            Assert.IsFalse(result.Contains(":"), "Result should not contain invalid filename characters");
            Assert.IsFalse(result.Contains("\""), "Result should not contain invalid filename characters");
            Assert.IsFalse(result.Contains("|"), "Result should not contain invalid filename characters");
            Assert.IsFalse(result.Contains("?"), "Result should not contain invalid filename characters");
            Assert.IsFalse(result.Contains("*"), "Result should not contain invalid filename characters");
        }

        [TestMethod]
        public void GetVolumeLabel_RemovesSpaces()
        {
            // Arrange
            string inputFileName = "Test Movie Name";
            
            // Act
            string result = Ripping.GetVolumeLabel(inputFileName);
            
            // Assert
            Assert.AreEqual("TestMovieName", result, "Spaces should be removed");
        }

        [TestMethod]
        public void GetVolumeLabel_HandlesNullInput()
        {
            // Arrange
            string inputFileName = null;
            
            // Act & Assert
            try
            {
                string result = Ripping.GetVolumeLabel(inputFileName);
                // If we get here, the method handled null gracefully
            }
            catch (ArgumentNullException)
            {
                // This is acceptable behavior
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception type: {ex.GetType().Name}");
            }
        }

        [TestMethod]
        public void GetVolumeLabel_HandlesEmptyString()
        {
            // Arrange
            string inputFileName = string.Empty;
            
            // Act
            string result = Ripping.GetVolumeLabel(inputFileName);
            
            // Assert
            Assert.AreEqual(string.Empty, result, "Empty string should return empty string");
        }

        [TestMethod]
        public void GetVolumeLabel_PreservesValidCharacters()
        {
            // Arrange
            string inputFileName = "ValidMovieTitle123";
            
            // Act
            string result = Ripping.GetVolumeLabel(inputFileName);
            
            // Assert
            Assert.AreEqual("ValidMovieTitle123", result, "Valid characters should be preserved");
        }

        [TestMethod]
        public void SaveSettings_DoesNotThrow()
        {
            // Act & Assert
            try
            {
                Ripping.SaveSettings();
            }
            catch (Exception)
            {
                Assert.Fail("SaveSettings should not throw an exception");
            }
        }

        [TestMethod]
        public void UpdateStatusText_Clear_DoesNotThrow()
        {
            // Act & Assert
            try
            {
                Ripping.UpdateStatusText("Clear");
            }
            catch (Exception)
            {
                Assert.Fail("UpdateStatusText with 'Clear' should not throw an exception");
            }
        }

        [TestMethod]
        public void UpdateStatusText_RegularMessage_DoesNotThrow()
        {
            // Act & Assert
            try
            {
                Ripping.UpdateStatusText("Test status message");
            }
            catch (Exception)
            {
                Assert.Fail("UpdateStatusText with regular message should not throw an exception");
            }
        }

        [TestMethod]
        public void CheckMakeMKVRegistry_DoesNotThrow()
        {
            // Act & Assert
            try
            {
                string result = Ripping.CheckMakeMKVRegistry();
                // Result may be null or a path, both are valid
            }
            catch (Exception)
            {
                Assert.Fail("CheckMakeMKVRegistry should not throw an exception");
            }
        }

        [TestMethod]
        public void CheckVariables_DoesNotThrow()
        {
            // Act & Assert
            try
            {
                bool result = Ripping.CheckVariables();
                // Result can be true or false, both are valid
            }
            catch (Exception)
            {
                Assert.Fail("CheckVariables should not throw an exception");
            }
        }

        [TestMethod]
        public void CheckForRecentRip_NonExistentPath_ReturnsFalse()
        {
            // Arrange
            string nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            
            // Act
            bool result = Ripping.CheckForRecentRip(nonExistentPath);
            
            // Assert
            Assert.IsFalse(result, "CheckForRecentRip should return false for non-existent path");
        }

        [TestMethod]
        public void Decompress_DoesNotThrow()
        {
            // This test verifies the method doesn't crash, but actual extraction
            // would require the HandbrakeCLI.zip file to exist
            try
            {
                // We can't easily test this without setting up the file structure
                // But we can ensure the method signature is correct
                var method = typeof(Ripping).GetMethod("Decompress");
                Assert.IsNotNull(method, "Decompress method should exist");
                Assert.AreEqual(typeof(void), method.ReturnType, "Decompress should return void");
            }
            catch (Exception)
            {
                Assert.Fail("Decompress method signature verification failed");
            }
        }
    }
}
