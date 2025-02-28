using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR2CP;
namespace testsForm
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestChangeDAndNCounterTrueDNFirst()
        {

            bool isComplete = false;
            try
            {
                double a = 12, b = 13;
                string testName = "test1";
                ChangeDAndNCounter counterC = new ChangeDAndNCounter();
                counterC.SaveOldCounterToHistory(testName);
                counterC.SaveNewCounterToOld(testName);
                counterC.ChangeDAndNCounterTrueDN(testName, a, b);
                isComplete = true;
            }
            catch
            {
                isComplete = false;
            }
            Assert.IsTrue(isComplete);

        }
        [TestMethod]
        public void TestChangeDAndNCounterTrueDNSecond()
        {
            bool isComplete = false;
            try
            {
                double a = 15, b = 17;
                string testName = "test1";
                ChangeDAndNCounter counterC = new ChangeDAndNCounter();
                counterC.SaveOldCounterToHistory(testName);
                counterC.SaveNewCounterToOld(testName);
                counterC.ChangeDAndNCounterTrueDN(testName, a, b);
                isComplete = true;
            }
            catch
            {
                isComplete = false;
            }
            Assert.IsTrue(isComplete);

        }
        [TestMethod]
        public void TestChangeDAndNCounterFalseDN()
        {
            bool isComplete = false;
            try
            {
                double a = 12, b = 13;
                string testName = "test1";
                ChangeDAndNCounter counterC = new ChangeDAndNCounter();
                counterC.SaveOldCounterToHistory(testName);
                counterC.SaveNewCounterToOld(testName);
                counterC.ChangeDAndNCounterFalseDN(testName, a, b);
                isComplete = true;
            }
            catch
            {
                isComplete = false;
            }
            Assert.IsTrue(isComplete);
        }
        [TestMethod]
        public void TestChangeDAndNCounterFalseD()
        {
            bool isComplete = false;
            try
            {
                double a = 15, b = 120;
                string testName = "test1";
                ChangeDAndNCounter counterC = new ChangeDAndNCounter();
                counterC.SaveOldCounterToHistory(testName);
                counterC.SaveNewCounterToOld(testName);
                counterC.ChangeDAndNCounterFalseD(testName, a, b);
                isComplete = true;
            }
            catch
            {
                isComplete = false;
            }
            Assert.IsTrue(isComplete);
        }
        [TestMethod]
        public void TestChangeDAndNCounterFalseN()
        {
            bool isComplete = false;
            try
            {
                double a = 200, b = 15;
                string testName = "test1";
                ChangeDAndNCounter counterC = new ChangeDAndNCounter();
                counterC.SaveOldCounterToHistory(testName);
                counterC.SaveNewCounterToOld(testName);
                counterC.ChangeDAndNCounterFalseN(testName, a, b);
                isComplete = true;
            }
            catch
            {
                isComplete = false;
            }
            Assert.IsTrue(isComplete);
        }
    }
}
