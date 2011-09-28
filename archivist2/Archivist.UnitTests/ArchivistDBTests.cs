using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Archivist.Data;
using System;

namespace Archivist.UnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ArchivistDBTests
    {
        public ArchivistDBTests()
        {


        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            using (IDbConnection connection = DataBuider.database.CreateOpenConnection())
            {
                using (IDbCommand command = DataBuider.database.CreateCommand("SELECT ID, EXT, NAME FROM EXTENSIONS", connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object fieldId = reader.GetInt32(0);
                            object fieldExt = reader.GetString(1);
                            Assert.IsNotNull(fieldExt);
                        }
                    }
                }
            }
        }
    }
}
