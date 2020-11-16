using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stexchange.Controllers;
using System;

namespace Tests
{
    [TestClass]
    public class ServerControllerTest
    {
        private static ServerController testObject;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            testObject = new ServerController();
        }
        [TestMethod]
        public void SessionExists_True()
        {
            //Arrange
            var testData = new Tuple<int, string>(29, "2846DF");
            long token = testObject.CreateSession(testData);
            //Act
            bool outcome = testObject.SessionExists(token);
            //Assert
            Assert.IsTrue(outcome);
            //Cleanup
            testObject.TerminateSession(token);
        }
        [TestMethod]
        public void SessionExists_False()
        {
            Assert.IsFalse(testObject.SessionExists(57));
        }
        [TestMethod]
        public void GetSessionData_True()
        {
            //Arrange
            var expected = new Tuple<int, string>(34, "7519YY");
            long token = testObject.CreateSession(expected);
            //Act
            bool outcome = testObject.GetSessionData(token, out Tuple<int, string> actual);
            //Assert
            Assert.IsTrue(outcome);
            Assert.AreEqual(expected, actual);
            //Cleanup
            testObject.TerminateSession(token);
        }
        [TestMethod]
        public void GetSessionData_False()
        {
            //Act
            bool outcome = testObject.GetSessionData(42, out Tuple<int, string> output);
            //Assert
            Assert.IsFalse(outcome);
            Assert.IsNull(output);
        }
        [TestMethod]
        public void TerminateSession_True()
        {
            //Arrange
            var testData = new Tuple<int, string>(17, "1145BV");
            long token = testObject.CreateSession(testData);
            //Act
            bool outcome = testObject.TerminateSession(token);
            bool checkRemoved = !testObject.SessionExists(token);
            //Assert
            Assert.IsTrue(outcome);
            Assert.IsTrue(checkRemoved);
        }
        [TestMethod]
        public void TerminateSession_False()
        {
            Assert.IsFalse(testObject.TerminateSession(98));
        }
    }
}
