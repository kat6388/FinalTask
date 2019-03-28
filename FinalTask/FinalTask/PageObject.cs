using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.Chrome;

namespace FinalTask
{
    class PageObject
    {
        private readonly IWebDriver driver;
        private readonly string url = @"https://www.gmail.com/";
        public string usernameUser1 = "seleniumtests10";
        public string passwordUser1 = "060788avavav";
        public string usernameUser2 = "seleniumtest2007";
        public string passwordUser2 = "060788avavav";

        public static string GenerateUniqueSubject()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            var subject = path.Substring(0, 4);
            return subject;
        }
        public string subjectToSend = GenerateUniqueSubject();

        public PageObject(IWebDriver browser)
        {
            driver = browser;
            driver.Manage().Window.Maximize();
            PageFactory.InitElements(browser, this);
        }

        //Login flow locators

        [FindsBy(How = How.Id, Using = "identifierId")]
        public IWebElement UsernameInput { get; set; }

        [FindsBy(How = How.Id, Using = "identifierNext")]
        public IWebElement UsernameNextButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@type='password']")]
        public IWebElement PasswordInput { get; set; }

        [FindsBy(How = How.Id, Using = "passwordNext")]
        public IWebElement PasswordNextButton { get; set; }

        //Gmail default page locators

        [FindsBy(How = How.XPath, Using = "//*[contains(@aria-label, 'Selenium')]")]
        public IWebElement LoggedUser { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id = 'gb_71']")]
        public IWebElement SignOutButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div[gh='cm']")]
        public IWebElement NewEmailButton { get; set; }

        //Send Email

        [FindsBy(How = How.XPath, Using = "//textarea[@aria-label='To']")]
        public IWebElement NewEmailToInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@aria-label='Subject']")]
        public IWebElement NewEmailSubjectInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@aria-label='Send ‪(Ctrl-Enter)‬']")]
        public IWebElement SendEmailButton { get; set; }


        [FindsBy(How = How.XPath, Using = "//div[@class='vh‬']>span")]
        public IWebElement EmailSentTextBox { get; set; }




        [FindsBy(How = How.XPath, Using = "//*[@data-tooltip='Sent']")]
        public IWebElement SentFolder { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[id= ':97']>span")]
        public IWebElement SentItem { get; set; }



        //[FindsBy(How = How.CssSelector, Using = "span[id = ':30']>span")]
        //public IWebElement ReceivedItem { get; set; }

        //[FindsBy(How = How.CssSelector, Using = "div[class = 'ae4 aDM']> div>div>table>tbody>tr>td:nth-child(2)")]
        //public IWebElement SelectEmailCheckbox { get; set; }

        //[FindsBy(How = How.CssSelector, Using = "div[class='bzn']>div>div:nth-child(2)>div:nth-child(3)")]
        //public IWebElement DeleteIcon { get; set; }


        [FindsBy(How = How.CssSelector, Using = "h2[class='hP']")]
        public IWebElement Subj { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div[class='iH bzn']>div>div>div[title='Delete']")]
        public IWebElement DeleteIcon { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@gh='mll']")]
        public IWebElement MoreLink { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@data-tooltip='Trash']")]
        public IWebElement TrashFolder { get; set; }

        //[FindsBy(How = How.CssSelector, Using = "td[id=':73']>div>div>div>span>span")]
        //public IWebElement RemovedEmailSubject { get; set; }

        public void WaitUntilElementDisplayed(IWebDriver chromeDriver, By by)
        {
            var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
            var element = wait.Until(condition =>
            {
                try
                {
                    var elementToBeDisplayed = chromeDriver.FindElement(by);
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        public void TakeScreenshot()
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            String fp = "D:\\" + "snapshot" + "_" + DateTime.Now.ToString("dd_MMMM_hh_mm_ss_tt") + ".png";
            screenshot.SaveAsFile(fp, ScreenshotImageFormat.Png);
        }

        public void NavigateTo()
        {
            driver.Navigate().GoToUrl(this.url);
        }
        public void Login(string username, string password)
        {
            UsernameInput.SendKeys(username);
            UsernameNextButton.Click();
            WaitUntilElementDisplayed(driver, By.XPath("//input[@type='password']"));
            PasswordInput.SendKeys(password);
            PasswordNextButton.Click();
            WaitUntilElementDisplayed(driver, By.XPath("//*[contains(@aria-label, 'Selenium')]"));
            Assert.IsTrue(LoggedUser.Displayed);
        }

        public void Logout()
        {
            WaitUntilElementDisplayed(driver, By.XPath("//*[contains(@aria-label, 'Selenium')]"));
            LoggedUser.Click();
            SignOutButton.Click();
            Assert.IsTrue(PasswordInput.Displayed);
        }

        public void VerifySentFolder(string username, string password)
        {
            Login(username, password);
            WaitUntilElementDisplayed(driver, By.CssSelector("div[gh='cm']"));
            NewEmailButton.Click();
            WaitUntilElementDisplayed(driver, By.XPath("//textarea[@aria-label='To']"));
            NewEmailToInput.SendKeys("seleniumtest2007@gmail.com");
            NewEmailSubjectInput.SendKeys(subjectToSend);
            SendEmailButton.Click();
            WaitUntilElementDisplayed(driver, By.XPath("//*[@data-tooltip='Sent']"));
            SentFolder.Click();
            List<IWebElement> emails = driver.FindElements(By.CssSelector("div[class = 'ae4 UI']> div>div>table>tbody>tr")).ToList();
            foreach (IWebElement email in emails)
            {
                if (email.Text.Contains(subjectToSend) == true)
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

        public void VerifyInboxFolder(string username, string password, string username1,  string password1)
        {
            Login(username, password);
            WaitUntilElementDisplayed(driver, By.CssSelector("div[gh='cm']"));
            NewEmailButton.Click();
            WaitUntilElementDisplayed(driver, By.XPath("//textarea[@aria-label='To']"));
            NewEmailToInput.SendKeys("seleniumtest2007@gmail.com");
            NewEmailSubjectInput.SendKeys(subjectToSend);
            WaitUntilElementDisplayed(driver, By.XPath("//*[@data-tooltip='Sent']"));
            SendEmailButton.Click();
            Thread.Sleep(1000);
            WaitUntilElementDisplayed(driver, By.XPath("//*[contains(@aria-label, 'Selenium')]"));
            LoggedUser.Click();
            WaitUntilElementDisplayed(driver, By.XPath("//*[@id = 'gb_71']"));
            SignOutButton.Click();
            driver.Quit();
            
            //var Driver = new ChromeDriver(@"D:\Automation");
            PageObject gmail = new PageObject(driver);
            NavigateTo();

            //Login(username1, password1);
            //List<IWebElement> emails = driver.FindElements(By.CssSelector("div[class = 'ae4 aDM']> div>div>table>tbody>tr")).ToList();
            //foreach (IWebElement email in emails)
            //{
            //    if (email.Text.Contains(sss) == true)
            //    {
            //        email.Click();
            //        break;
            //    }
            //    else
            //    {
            //        driver.Close();
            //    }
            //}
        }

        public void DeleteEmailVerifyTrashFolder(string username, string password)
        {
            Login(username, password);
            List<IWebElement> emails = driver.FindElements(By.CssSelector("div[class = 'ae4 aDM']> div>div>table>tbody>tr")).ToList();
            emails[1].Click();
            var subj = Subj.Text.ToString();
            MoreLink.Click();
            DeleteIcon.Click();
            TrashFolder.Click();
            List<IWebElement> emailsTrash = driver.FindElements(By.CssSelector("div[class = 'ae4 UI']> div>div>table>tbody>tr")).ToList();
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
    }
}

