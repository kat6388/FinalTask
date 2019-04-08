using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace FinalTask
{
    class Waiters
    {
        public bool WaitUntilElementDisplayed(IWebDriver chromeDriver, By by)
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
            return true;
        }

        public void IsElementdisplayed(IWebDriver chromeDriver, By by)
        {
            Assert.IsTrue(WaitUntilElementDisplayed(chromeDriver,by), "Element is not displayed");
        }
    }
}
