using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Team7ADProject.Controllers;
using Team7ADProject.ViewModels.GenerateReport;

namespace Team7ADProject.Tests.Controllers
{
    [TestClass]
    public class GenerateReportControllerTest
    {
        [TestMethod]
        public void TestInitialReportView()
        {
            var controller = new GenerateReportController();
            var result = controller.GenerateDashboard() as ViewResult;
            var model = result.Model as GenerateReportViewModel;
            Assert.IsNotNull(result);
            Assert.AreEqual("Disbursements", model.module);
            Assert.AreEqual(10, model.selectentcategory.Count);
        }

        [TestMethod]
        public void TestFilteredReportView()
        {
            var controller = new GenerateReportController();
            var result = controller.GenerateDashboard(new DateTime(2017, 03, 03), new DateTime(2018, 03, 03), "Requests",
                new List<String>(new string[] { "Pen","Clip" }), new List<String>(new string[] { "ACCT","BUSI" }), null, null) 
                as ViewResult;
            var model = result.Model as GenerateReportViewModel;
                       
            Assert.IsNotNull(result);
            

        }
    }
}
