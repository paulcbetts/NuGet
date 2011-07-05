﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test;

namespace NuGet.VisualStudio.Test {
    [TestClass]
    public class PathHelperTest {

        [TestMethod]
        public void ThrowsIfInputIsNull() {
            // Act & Assert
            ExceptionAssert.ThrowsArgNull(() => PathHelper.SmartTruncate(null, 10), "path");
        }

        [TestMethod]
        public void ThrowsIfMaxLengthIsLessThan6() {
            // Act & Assert
            ExceptionAssert.ThrowsArgOutOfRange(() => PathHelper.SmartTruncate("", 5), "maxWidth", 6, null, true);
            ExceptionAssert.ThrowsArgOutOfRange(() => PathHelper.SmartTruncate("", -4), "maxWidth", 6, null, true);
        }

        [TestMethod]
        public void ReturnsTheSameStringIfItIsEqualToMaxWidthValue() {
            // Arrange
            string input = "abcdef";
            int maxWidth = 6;

            // Act
            string output = PathHelper.SmartTruncate(input, maxWidth);

            // Assert
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void ReturnsTheSameStringIfItIsShorterThanMaxWidthValue() {
            // Arrange
            string input = @"c:\user\documents\projects";
            int maxWidth = 30;

            // Act
            string output = PathHelper.SmartTruncate(input, maxWidth);

            // Assert
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TruncateIfInputIsLongerThanMaxWidth() {
            // Arrange
            string input = @"c:\user\documents\projects";
            int maxWidth = 20;

            // Act
            string output = PathHelper.SmartTruncate(input, maxWidth);

            // Assert
            Assert.AreEqual(@"c:\...\projects\", output);
        }

        [TestMethod]
        public void TruncateIfInputIsLongerThanMaxWidth2() {
            // Arrange
            string input = @"c:\user\documents\projects\";
            int maxWidth = 26;

            // Act
            string output = PathHelper.SmartTruncate(input, maxWidth);

            // Assert
            Assert.AreEqual(@"c:\...\projects\", output);
        }

        [TestMethod]
        public void TruncateFolderNameIfItIsTooLong() {
            // Arrange
            string input = @"c:\thisisaverylongname";
            int maxWidth = 10;

            // Act
            string output = PathHelper.SmartTruncate(input, maxWidth);

            // Assert
            Assert.IsTrue(output.Length == maxWidth);
            Assert.AreEqual(@"c:\...ame\", output);
        }

        [TestMethod]
        public void EscapePSPathTest() {
            TestEscapePSPath("", "''");
            TestEscapePSPath("abc", "'abc'");
            TestEscapePSPath("a$$b", "'a$$b'");
            TestEscapePSPath("'a$$'b", "\"'a`$`$'b\"");
            TestEscapePSPath("hello world", "'hello world'");
            TestEscapePSPath("Gun 'n Roses", "\"Gun 'n Roses\"");
            TestEscapePSPath("Hello [ Kitty ]", "'Hello `[ Kitty `]'");
            TestEscapePSPath("Foo []", "'Foo `[`]'");
            TestEscapePSPath("Fo'o []", "\"Fo'o `[`]\"");
            TestEscapePSPath("Bar [", "'Bar `['");
            TestEscapePSPath("Bar ]", "'Bar `]'");
            TestEscapePSPath(@"c:\users\name\Foo]\Console.sln", @"'c:\users\name\Foo`]\Console.sln'");
        }

        private static void TestEscapePSPath(string input, string expectedOutput) {
            string output = PathHelper.EscapePSPath(input);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}