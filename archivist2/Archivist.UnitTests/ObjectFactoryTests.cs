using System.IO;


namespace ArticleListings.ObjectFactory
{
    using ArticleListings.IO;
    using ArticleListings.ObjectFactory;
    using Archivist.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Archivist.ObjectFactory;

   [TestClass]
   public class ObjectFactoryTests
   {
      [ClassInitialize()]
       public static void LocateObjectFile(TestContext testContext)
      {
          //File.Copy(ValidatedPath.ExistingFilePath(@"Objects.xml"), ValidatedPath.AbsolutePath("Objects.xml"), true);
      }

      [TestMethod]
      public void Construction()
      {
         Assert.IsNotNull(ObjectFactory.Instance);
      }

      [TestMethod]
      public void SingletonAccess()
      {
         const string objectName = "MockObject";

         MockObject o1 = (MockObject)ObjectFactory.Instance.GetObject(objectName);
         MockObject o2 = (MockObject)ObjectFactory.Instance.GetObject(objectName);

         Assert.IsNotNull(o1);
         Assert.AreEqual(o1, o2);
      }

      [TestMethod]
      [ExpectedException(typeof(FileNotFoundException))]
      public void MissingObjectFile()
      {
         File.Move("Objects.xml", "Objects2.xml");
         File.Delete("Objects.xml");
         
         ObjectFactory.Instance.GetType();
      }

      [ClassCleanup()]
      public static void MyClassCleanup() 
      {
          if (!File.Exists("Objects.xml"));
          File.Move("Objects2.xml", "Objects.xml"); }
   }
}

