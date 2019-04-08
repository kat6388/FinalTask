using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace FinalTask
{
    class Login
    {
        private IWebDriver driver;
        Waiters wait = new Waiters();
        public const string avatarLocator = "//*[contains(@aria-label, 'Selenium')]";
        public const string passwordInputLocator = "//input[@type='password']";

        public Login(IWebDriver driver)
        {
            this.driver = driver;
            driver.Manage().Window.Maximize();
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.Id, Using = "identifierId")]
        public IWebElement UsernameInput { get; set; }

        [FindsBy(How = How.Id, Using = "identifierNext")]
        public IWebElement UsernameNextButton { get; set; }

        [FindsBy(How = How.XPath, Using = passwordInputLocator)]
        public IWebElement PasswordInput { get; set; }

        [FindsBy(How = How.Id, Using = "passwordNext")]
        public IWebElement PasswordNextButton { get; set; }

        [FindsBy(How = How.XPath, Using = avatarLocator)]
        public IWebElement LoggedUser { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id = 'gb_71']")]
        public IWebElement SignOutButton { get; set; }

        public void TakeScreenshot()
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            var fp = "D:\\" + "snapshot" + "_" + DateTime.Now.ToString("dd_MMMM_hh_mm_ss_tt") + ".png";
            screenshot.SaveAsFile(fp, ScreenshotImageFormat.Png);
        }

        public void NavigateTo()
        {
            driver.Navigate().GoToUrl("https://www.gmail.com/");
        }

        public Main PopulateLogin(string username, string password)
        {
            UsernameInput.SendKeys(username);
            UsernameNextButton.Click();
            wait.WaitUntilElementDisplayed(driver, By.XPath(passwordInputLocator));
            PasswordInput.SendKeys(password);
            PasswordNextButton.Click();
            return new Main(driver);
        }

        public void IsUserLoggedIn()
        {
            wait.WaitUntilElementDisplayed(driver, By.XPath(avatarLocator));
            Assert.IsTrue(LoggedUser.Displayed);
        }

        public void Logout()
        {
            LoggedUser.Click();
            SignOutButton.Click();
        }

        public void IsUserSignedOut()
        {
            wait.WaitUntilElementDisplayed(driver, By.XPath(passwordInputLocator));
            Assert.IsTrue(PasswordInput.Displayed);
        }
    }
}
