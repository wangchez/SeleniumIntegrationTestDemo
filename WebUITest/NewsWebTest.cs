using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WebUITest
{
    [TestClass]
    public class NewsWebTest
    {
        private IWebDriver webDriver;
        private string testMainPageUrl = "http://localhost:26109/NewsSmith";

        public NewsWebTest()
        {
            this.webDriver = new ChromeDriver();
            //this.webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1)); //Implicit waits
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            webDriver.Navigate().GoToUrl(testMainPageUrl);

            loginProcess();
        }

        [TestMethod]
        public void TestAddNews_success()
        {
            IWebElement addBtn = webDriver.FindElement(By.XPath("//nav/button[text()='新增稿單']"));

            string current = webDriver.CurrentWindowHandle;
            PopupWindowFinder finder = new PopupWindowFinder(webDriver); //pop up window finder
            string newHandle = finder.Click(addBtn);
            webDriver.SwitchTo().Window(newHandle);

            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)); //Explicit waits
            //wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement news = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h4.page-header"))); //get the serial number
            String newsSerialNumber = news.Text;
            webDriver.SwitchTo().Window(current); //switch back
            var additionNews = wait.Until(ExpectedConditions.ElementExists(By.XPath(string.Format("//table/thead/tr/th[a='{0}']", newsSerialNumber))));

            Assert.IsNotNull(additionNews);
        }

        [TestMethod]
        public void TestDeleteNews_success()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(ElementNotVisibleException));

            IWebElement deleteBtn = webDriver.FindElement(By.XPath("//nav/button[text()='刪除']"));

            By searchingTableByXPath = By.XPath("//table[./thead/tr/th/span/a/strong/text()='']");
            IWebElement table = wait.Until(ExpectedConditions.ElementExists(searchingTableByXPath));

            By searchingCheckBoxByXPath = By.XPath("./thead/tr/th/a[@href='#IsCheck']");
            IWebElement checkedBox = wait.Until(driver => table.FindElement(searchingCheckBoxByXPath));

            wait.Until((driver) => { checkedBox.Click(); return true; });
            deleteBtn.Click();

            By searchingAcceptBtnById = By.Id("btnConfirm_ConfirmBox");
            IWebElement acceptDeleteBtn = wait.Until(ExpectedConditions.ElementIsVisible(searchingAcceptBtnById));

            acceptDeleteBtn.Click();

            IWebElement elementIsNotAvailable = null;
            try
            {
                elementIsNotAvailable = ExpectedConditions.ElementExists(searchingCheckBoxByXPath).Invoke(webDriver);
            }
            catch { };

            Assert.IsNull(elementIsNotAvailable);

            webDriver.Quit();
        }

        private void loginProcess()
        {
            IWebElement accountInput = webDriver.FindElement(By.Id("UserId"));
            IWebElement passwordInput = (IWebElement)((IJavaScriptExecutor)webDriver).ExecuteScript("return $('#Password')[0]");
            IWebElement loginBtn = webDriver.FindElement(By.ClassName("btn"));

            accountInput.SendKeys("19990");
            passwordInput.SendKeys("19990");

            loginBtn.Click();
        }
    }
}
