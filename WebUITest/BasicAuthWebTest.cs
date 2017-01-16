using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebUITest
{
    [TestClass]
    public class BasicAuthWebTest
    {
        private IWebDriver webDriver;
        private string testLoginPageUrl = "http://localhost:26109/NewsSmith/Account/Login";

        public BasicAuthWebTest()
        {
            this.webDriver = new ChromeDriver(); //FireFox, IE, etc.

            webDriver.Navigate().GoToUrl(testLoginPageUrl);
        }

        [TestMethod]
        public void TestLoginProcess_success()
        {
            IWebElement accountInput = webDriver.FindElement(By.Id("UserId"));
            IWebElement passwordInput = (IWebElement)((IJavaScriptExecutor)webDriver).ExecuteScript("return $('#Password')[0]");
            IWebElement loginBtn = webDriver.FindElement(By.ClassName("btn"));

            accountInput.SendKeys("19990");
            passwordInput.SendKeys("19990");

            loginBtn.Click();

            Cookie loginCookie = webDriver.Manage().Cookies.GetCookieNamed(".AspNet.ApplicationCookie");

            Assert.IsNotNull(loginCookie);

            webDriver.Quit();
        }

        [TestMethod]
        public void TestLoginProcess_fail()
        {
            IWebElement accountInput = webDriver.FindElement(By.Id("UserId"));
            IWebElement passwordInput = (IWebElement)((IJavaScriptExecutor)webDriver).ExecuteScript("return $('#Password')[0]");
            IWebElement loginBtn = webDriver.FindElement(By.ClassName("btn"));

            accountInput.SendKeys("19990");
            passwordInput.SendKeys("xxxxx");

            loginBtn.Click();

            Cookie loginCookie = webDriver.Manage().Cookies.GetCookieNamed(".AspNet.ApplicationCookie");

            Assert.IsNull(loginCookie);
        }
    }
}
