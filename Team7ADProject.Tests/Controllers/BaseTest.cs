using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Team7ADProject.Tests.Controllers
{
    [TestClass]
    public class BaseTest
    {
        protected DataContextProvider contextProv = new LogicDB();

        [TestInitialize]
        public void Setup()
        {

        }
    }
}
