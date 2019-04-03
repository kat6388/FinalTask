using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FinalTask
{
    class Main
    {
        private IWebDriver driver;
        Waiters wait = new Waiters();
        public string subjToSent = StringUtils.GenerateUniqueSubject();

        public const string ToInputLocator = "//textarea[@aria-label='To']";
        public const string NewEmailButtonLocator = "div[gh='cm']";
        public const string SentFolderLocator = "//*[@data-tooltip='Sent']";
        public const string EmailsInSentFolderLocator = "div[class = 'ae4 UI']> div>div>table>tbody>tr";
        public const string EmailsInInboxLocator = "div[class = 'ae4 aDM']> div>div>table>tbody>tr";
        public const string EmailsInTrashFolderLocator = "div[class = 'ae4 UI']> div>div>table>tbody>tr";


        public Main(IWebDriver driver)
        {
            this.driver = driver;
            driver.Manage().Window.Maximize();
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.CssSelector, Using = NewEmailButtonLocator)]
        public IWebElement NewEmailButton { get; set; }

        [FindsBy(How = How.XPath, Using = ToInputLocator)]
        public IWebElement ToInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@aria-label='Subject']")]
        public IWebElement SubjectInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@aria-label='Send ‪(Ctrl-Enter)‬']")]
        public IWebElement SendEmailButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='vh‬']>span")]
        public IWebElement EmailSentTextBox { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@data-tooltip='Sent']")]
        public IWebElement SentFolderIcon { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h2[class='hP']")]
        public IWebElement Subj { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div[class='iH bzn']>div>div>div[title='Delete']")]
        public IWebElement DeleteIcon { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@gh='mll']")]
        public IWebElement MoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@data-tooltip='Trash']")]
        public IWebElement TrashFolderIcon { get; set; }

        public void TakeScreenshot()
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            var fp = "D:\\" + "snapshot" + "_" + DateTime.Now.ToString("dd_MMMM_hh_mm_ss_tt") + ".png";
            screenshot.SaveAsFile(fp, ScreenshotImageFormat.Png);
        }

        public void SendEmail()
        {
            wait.WaitUntilElementDisplayed(driver, By.CssSelector(NewEmailButtonLocator));
            NewEmailButton.Click();
            wait.WaitUntilElementDisplayed(driver, By.XPath(ToInputLocator));
            ToInput.SendKeys("seleniumtest2007@gmail.com");
            SubjectInput.SendKeys(subjToSent);
            SendEmailButton.Click();
            wait.WaitUntilElementDisplayed(driver, By.XPath(SentFolderLocator));
        }

        public void SearchInSentFolder()
        {
            SentFolderIcon.Click();
            List<IWebElement> emails = driver.FindElements(By.CssSelector(EmailsInSentFolderLocator)).ToList();
            foreach (IWebElement email in emails)
            {
                if (email.Text.Contains(subjToSent) == true)
                {
                    email.Click();
                    break;
                }
                else
                {
                    driver.Close();
                }
            }
        }

        public void DeleteEmail()
        {
            Thread.Sleep(5000);
            List<IWebElement> emails = driver.FindElements(By.CssSelector(EmailsInInboxLocator)).ToList();
            emails[4].Click();
            var subj = Subj.Text.ToString();
            MoreLink.Click();
            DeleteIcon.Click();
            TrashFolderIcon.Click();
            List<IWebElement> emailsTrash = driver.FindElements(By.CssSelector(EmailsInTrashFolderLocator)).ToList();
            foreach (IWebElement email in emailsTrash)
            {
                if (email.Text.Contains(subj) == true)
                {
                    email.Click();
                    break;
                }
                else
                {
                    driver.Close();
                }
            }
        }

        public void VerifyInboxFolder()
        {
            List<IWebElement> emails = driver.FindElements(By.CssSelector(EmailsInInboxLocator)).ToList();
            foreach (IWebElement email in emails)
            {
                if (email.Text.Contains(subjToSent) == true)
                {
                    email.Click();
                    break;
                }
                else
                {
                    driver.Close();
                }
            }
        }
    }
}
