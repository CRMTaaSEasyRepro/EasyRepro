﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateOpportunityUCI : ExtentReportManager
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestCreateOpportunity()
        {
            CreateTest("Test_01", "Verify the ability to create a new opportunity");
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("name", TestSettings.GetRandomString(5,10));

                xrmApp.Entity.Save();

                LogTestStep(AventStack.ExtentReports.Status.Pass, "Passed");

                test.Pass("Test Passed");

            }
            
        }

        [TestMethod]
        public void UCITestCreateOpportunity_SetHeaderDate()
        {
            CreateTest("Test_02", "Verify the ability to create a new opportunity with estimated close date");
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("name", "Opportunity " + TestSettings.GetRandomString(5, 10));

                DateTime expectedDate = DateTime.Today.AddDays(10);

                xrmApp.Entity.SetHeaderValue("estimatedclosedate", expectedDate);

                var commandResult = xrmApp.Entity.GetHeaderValue(new DateTimeControl("estimatedclosedate"));
                DateTime? date = commandResult;
                Assert.AreEqual(expectedDate, date);
                LogTestStep(AventStack.ExtentReports.Status.Pass, "Passed");
            }
        }

        [TestMethod]
        public void UCITestCreateOpportunity_ClearHeaderDate()
        {
            CreateTest("Test_02", "Verify the ability to create a new opportunity after clearing the estimated close date");
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("name", "Opportunity " + TestSettings.GetRandomString(5, 10));

                DateTime expectedDate = DateTime.Today.AddDays(10);

                xrmApp.Entity.SetHeaderValue("estimatedclosedate", expectedDate);

                var control = new DateTimeControl("estimatedclosedate");
                DateTime? date = xrmApp.Entity.GetHeaderValue(control);
                Assert.AreEqual(expectedDate, date);
                LogTestStep(AventStack.ExtentReports.Status.Pass, "Passed");

                xrmApp.Entity.ClearHeaderValue(control);
                date = xrmApp.Entity.GetHeaderValue(control);
                Assert.AreEqual(null, date);
                LogTestStep(AventStack.ExtentReports.Status.Pass, "Passed");
            }
        }

    }
}