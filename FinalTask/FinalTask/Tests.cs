using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using Allure.Commons;
using Allure.NUnit.Attributes;
using NUnit.Framework.Internal;

namespace FinalTask
{
    [TestFixture]

    public class Tests : AllureReport
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }

        private static IEnumerable<TestCaseData> DivideCases
        {
            get
            {
                var users = new Users().ParseXML();

                foreach (var user in users)
                {
                    yield return new TestCaseData(user);
                }
            }
        }

        [SetUp]
        public void SetupTest()
        {
            Driver = new ChromeDriver(@"D:\Automation");
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(300));
        }

        [TearDown]
        public void TearDownTest()
        {
            AllureLifecycle.Instance.RunStep(() =>
            {
                TestContext.Error.WriteLine(
                    $"Test {TestExecutionContext.CurrentContext.CurrentTest.FullName}\" is stopping...");
                PageObject gmail = new PageObject(Driver);
                gmail.TakeScreenshot();
            });
        }

        [Test, TestCaseSource("DivideCases")]
        [AllureSubSuite("Login")]
        [AllureSeverity(Allure.Commons.Model.SeverityLevel.Critical)]
        [AllureLink("ID-1(2)")]
        [AllureTest]
        [AllureOwner("Katya Astakhova")]
        public void Login_User_Then_Logout(User user)
        {
            PageObject gmail = new PageObject(Driver);
            var username = user.username;
            var password = user.password;
            gmail.NavigateTo();
            gmail.Login(username, password);
            gmail.Logout();
            Driver.Close();
        }

        [Test]
        [AllureSubSuite("SentEmail")]
        [AllureSeverity(Allure.Commons.Model.SeverityLevel.Critical)]
        [AllureLink("ID-3")]
        [AllureTest]
        [AllureOwner("Katya Astakhova")]
        public void Verify_Emails_In_Sent_Folder()
        {
            PageObject gmail = new PageObject(Driver);
            var username = "seleniumtests10";
            var password = "060788avavav";
            gmail.NavigateTo();
            gmail.VerifySentFolder(username, password);
        }

        //[Test]
        //public void Verify_Inbox_Folder()
        //{
        //    PageObject gmailSession1 = new PageObject(Driver);
            
        //    gmailSession1.NavigateTo();
        //    gmailSession1.VerifyInboxFolder(gmailSession1.usernameUser1, gmailSession1.passwordUser1,
        //        gmailSession1.usernameUser2, gmailSession1.passwordUser2);
        //}

        [Test]
        [AllureSubSuite("RemoveEmail")]
        [AllureSeverity(Allure.Commons.Model.SeverityLevel.Critical)]
        [AllureLink("ID-3")]
        [AllureTest]
        [AllureOwner("Katya Astakhova")]
        public void Verify_Removed_Email_In_TrashFolder()
        {
            PageObject gmail = new PageObject(Driver);
            gmail.NavigateTo();
            gmail.DeleteEmailVerifyTrashFolder(gmail.usernameUser2, gmail.passwordUser2);
        }
    }
}
