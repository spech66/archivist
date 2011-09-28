using System;
using System.IO;


namespace ArticleListings.IO.UnitTests
{
    using ArticleListings.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Archivist.IO;

   [TestClass]
   public class ValidatedPathTests
   {
      [TestMethod]
      public void RootRelativePath()
      {
         string relativePath = "test";
         string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

         Assert.AreEqual(ValidatedPath.AbsolutePath(relativePath), expectedPath);
      }

      [TestMethod]
      public void RootAbsolutePath()
      {
         string path = AppDomain.CurrentDomain.BaseDirectory;

         Assert.AreEqual(ValidatedPath.AbsolutePath(path),path);
      }

      [TestMethod]
      public void AbsoluteShortPath()
      {
         string path = System.Environment.GetEnvironmentVariable("windir");

         Assert.IsTrue(Directory.Exists(path));
         Assert.IsFalse(path.Contains(" "));

         string actualPath = ValidatedPath.AbsoluteShortPath(path);

         Assert.AreEqual(actualPath, path);
      }

      [TestMethod]
      [ExpectedException(typeof(InvalidPathException))]
      public void InvalidAbsoluteShortPath()
      {
         string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "t est");
         Assert.IsFalse(Directory.Exists(path));
         Assert.IsTrue(path.Contains(" "));

         ValidatedPath.AbsoluteShortPath(path);
      }

      [TestMethod]
      public void RootAnExistingFolderPath()
      {
         string relativePath = "";
         string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

         Assert.AreEqual(ValidatedPath.ExistingFolderPath(relativePath), expectedPath);
      }

      [TestMethod]
      [ExpectedException(typeof(DirectoryNotFoundException))]
      public void NonExistantFolderPath()
      {
         ValidatedPath.ExistingFolderPath(Path.GetRandomFileName());
      }

      [TestMethod]
      public void RootAnExistingFilePath()
      {
         string relativePath = @"TestFile.txt";
         string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

         Assert.AreEqual(ValidatedPath.ExistingFilePath(relativePath), expectedPath);
      }

      [TestMethod]
      [ExpectedException(typeof(FileNotFoundException))]
      public void NonExistantFilePath()
      {
         ValidatedPath.ExistingFilePath(Path.GetRandomFileName());
      }
   }
}

