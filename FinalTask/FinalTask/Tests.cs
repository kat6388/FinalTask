using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using Allure.Commons;
using Allure.NUnit.Attributes;
using NUnit.Framework.Internal;
using OpenQA.Selenium.Firefox;
using System.Threading;

namespace FinalTask
{
    [TestFixture]

    public class Tests : AllureReport
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }

        public const string usernameUser1 = "seleniumtests10";
        public const string passwordUser1 = "060788avavav";
        public const string usernameUser2 = "seleniumtest2007";
        public const string passwordUser2 = "060788avavav";


        public enum Browsers
        {
            Chrome,
            Firefox
        }

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
            Browsers value = Browsers.Chrome;
            if (value == Browsers.Chrome)
            {
                Driver = new ChromeDriver(@"D:\Automation");
                Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(300));
            }
            else if (value == Browsers.Firefox)
            {
                Driver = new FirefoxDriver(@"D:\Automation");
                Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(300));
            }
        }

        [TearDown]
        public void TearDownTest()
        {
            AllureLifecycle.Instance.RunStep(() =>
            {
                TestContext.Error.WriteLine(
                    $"Test {TestExecutionContext.CurrentContext.CurrentTest.FullName}\" is stopping...");
                Login loginPage = new Login(Driver);
                loginPage.TakeScreenshot();
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
            Login loginPage = new Login(Driver);
            var username = user.username;
            var password = user.password;
            loginPage.NavigateTo();
            loginPage.PopulateLogin(username, password);
            loginPage.IsUserLoggedIn();
            loginPage.Logout();
            loginPage.IsUserSignedOut();
        }

        [Test]
        [AllureSubSuite("SentEmail")]
        [AllureSeverity(Allure.Commons.Model.SeverityLevel.Critical)]
        [AllureLink("ID-3")]
        [AllureTest]
        [AllureOwner("Katya Astakhova")]
        public void SendEmail()
        {
            Login loginPage = new Login(Driver);
            loginPage.NavigateTo();
            Main mainPage = loginPage.PopulateLogin(usernameUser1, passwordUser1);
            mainPage.SendEmail();
            mainPage.SearchInSentFolder();
        }

        [Test]
        [AllureSubSuite("ReceiveEmail")]
        [AllureSeverity(Allure.Commons.Model.SeverityLevel.Critical)]
        [AllureLink("ID-4")]
        [AllureTest]
        [AllureOwner("Katya Astakhova")]
        public void VerifyInbox()
        {
            Login loginPage = new Login(Driver);
            loginPage.NavigateTo();
            Main mainPage = loginPage.PopulateLogin(usernameUser1, passwordUser1);
            mainPage.SendEmail();
            Thread.Sleep(5000);
            loginPage.Logout();
            var  driver = new ChromeDriver();
            Login loginPage1 = new Login(driver);
            loginPage1.NavigateTo();
            Main mainPage1 = loginPage1.PopulateLogin(usernameUser2, passwordUser2);
            mainPage1.VerifyInboxFolder();
        }

        [Test]
        [AllureSubSuite("DeleteEmail")]
        [AllureSeverity(Allure.Commons.Model.SeverityLevel.Critical)]
        [AllureLink("ID-5")]
        [AllureTest]
        [AllureOwner("Katya Astakhova")]
        public void DeleteEmail()
        {
            Login loginPage = new Login(Driver);
            loginPage.NavigateTo();
            Main mainPage = loginPage.PopulateLogin(usernameUser2, passwordUser2);
            mainPage.DeleteEmail();
        }
    }
}
