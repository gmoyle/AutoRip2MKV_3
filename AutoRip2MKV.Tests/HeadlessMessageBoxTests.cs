using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace AutoRip2MKV.Tests
{
    [TestClass]
    public class HeadlessMessageBoxTests
    {
        [TestMethod]
        public void HeadlessMessageBox_Show_DoesNotHang()
        {
            // This test ensures that MessageBox calls don't hang in CI environments
            try
            {
                // This should not hang regardless of environment
                var result = HeadlessMessageBox.Show("Test message", "Test caption", MessageBoxButtons.OK);
                
                // Result should be OK for this button type
                Assert.AreEqual(DialogResult.OK, result, "Should return OK for OK button type");
            }
            catch (Exception ex)
            {
                Assert.Fail($"HeadlessMessageBox.Show should not throw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void HeadlessMessageBox_Show_ReturnsCorrectDefaultForYesNo()
        {
            try
            {
                var result = HeadlessMessageBox.Show("Test message", "Test caption", MessageBoxButtons.YesNo);
                
                // In headless mode, should return Yes as default
                // In interactive mode, will depend on user interaction or timeout
                Assert.IsTrue(result == DialogResult.Yes || result == DialogResult.No, 
                    "Should return either Yes or No for YesNo button type");
            }
            catch (Exception ex)
            {
                Assert.Fail($"HeadlessMessageBox.Show should not throw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void HeadlessMessageBox_SafeDeleteFile_DoesNotThrow()
        {
            // Create a temporary file for testing
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "test content");

            try
            {
                // This should not throw regardless of environment
                HeadlessMessageBox.SafeDeleteFile(tempFile, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                
                // File should be deleted
                Assert.IsFalse(File.Exists(tempFile), "File should be deleted");
            }
            catch (Exception ex)
            {
                // Clean up in case of failure
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
                Assert.Fail($"SafeDeleteFile should not throw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void HeadlessMessageBox_SafeDeleteDirectory_DoesNotThrow()
        {
            // Create a temporary directory for testing
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            
            // Create a file in the directory
            string testFile = Path.Combine(tempDir, "test.txt");
            File.WriteAllText(testFile, "test content");

            try
            {
                // This should not throw regardless of environment
                HeadlessMessageBox.SafeDeleteDirectory(tempDir, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                
                // Directory should be deleted
                Assert.IsFalse(Directory.Exists(tempDir), "Directory should be deleted");
            }
            catch (Exception ex)
            {
                // Clean up in case of failure
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                Assert.Fail($"SafeDeleteDirectory should not throw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void HeadlessMessageBox_SafeDeleteFile_HandlesNonExistentFile()
        {
            string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            
            try
            {
                // Should not throw even if file doesn't exist
                HeadlessMessageBox.SafeDeleteFile(nonExistentFile, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
            }
            catch (Exception ex)
            {
                Assert.Fail($"SafeDeleteFile should handle non-existent files gracefully: {ex.Message}");
            }
        }

        [TestMethod]
        public void HeadlessMessageBox_SafeDeleteDirectory_HandlesNonExistentDirectory()
        {
            string nonExistentDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            
            try
            {
                // Should not throw even if directory doesn't exist
                HeadlessMessageBox.SafeDeleteDirectory(nonExistentDir, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
            }
            catch (Exception ex)
            {
                Assert.Fail($"SafeDeleteDirectory should handle non-existent directories gracefully: {ex.Message}");
            }
        }
    }
}
