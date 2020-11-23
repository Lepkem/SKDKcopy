using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stexchange.Controllers;
using System;

namespace Tests
{
    [TestClass]
    public class ServerControllerTest
    {
        [TestMethod]
        public void SessionExists_True()
        {
            //Arrange
            var testData = new Tuple<int, string>(29, "2846DF");
            long token = ServerController.CreateSession(testData);
            //Act
            bool outcome = ServerController.SessionExists(token);
            //Assert
            Assert.IsTrue(outcome);
            //Cleanup
            ServerController.TerminateSession(token);
        }
        [TestMethod]
        public void SessionExists_False()
        {
            Assert.IsFalse(ServerController.SessionExists(57));
        }
        [TestMethod]
        public void GetSessionData_True()
        {
            //Arrange
            var expected = new Tuple<int, string>(34, "7519YY");
            long token = ServerController.CreateSession(expected);
            //Act
            bool outcome = ServerController.GetSessionData(token, out Tuple<int, string> actual);
            //Assert
            Assert.IsTrue(outcome);
            Assert.AreEqual(expected, actual);
            //Cleanup
            ServerController.TerminateSession(token);
        }
        [TestMethod]
        public void GetSessionData_False()
        {
            //Act
            bool outcome = ServerController.GetSessionData(42, out Tuple<int, string> output);
            //Assert
            Assert.IsFalse(outcome);
            Assert.IsNull(output);
        }
        [TestMethod]
        public void TerminateSession_True()
        {
            //Arrange
            var testData = new Tuple<int, string>(17, "1145BV");
            long token = ServerController.CreateSession(testData);
            //Act
            bool outcome = ServerController.TerminateSession(token);
            bool checkRemoved = !ServerController.SessionExists(token);
            //Assert
            Assert.IsTrue(outcome);
            Assert.IsTrue(checkRemoved);
        }
        [TestMethod]
        public void TerminateSession_False()
        {
            Assert.IsFalse(ServerController.TerminateSession(98));
        }
    }
}
