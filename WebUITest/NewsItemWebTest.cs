using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace WebUITest
{
    [TestClass]
    public class NewsItemWebTest
    {
        private IWebDriver webDriver;
        private string testDetailPageUrl = "http://localhost:26109/NewsSmith/News/Home?newsId=45ebbd46-da88-40bc-8bdb-8476867d8b28";


        public NewsItemWebTest()
        {
            this.webDriver = new ChromeDriver();
            this.webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            webDriver.Navigate().GoToUrl(testDetailPageUrl);

            loginProcess();
        }

        [TestMethod]
        public void TestAddContentAndCalculateWords_success()
        {
            IClock clock = new SystemClock();
            WebDriverWait wait = new WebDriverWait(clock, webDriver, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
           
            DateTime later = clock.LaterBy(wait.PollingInterval);
            IWebElement addContentBtn = webDriver.FindElement(By.XPath("//nav/button[text()='文字']"));
            IWebElement wordsCountLabel = webDriver.FindElement(By.CssSelector("label.lead-real"));
            addContentBtn.Click();
            IWebElement contentInput = wait.Until(ExpectedConditions.ElementExists(By.Name("txtContent")));
            string testString = "This is a selenium test";
            contentInput.SendKeys(testString);
            
            Assert.IsTrue(wordCountLogic(testString) == Convert.ToInt32(wordsCountLabel.Text));
        }

        [TestMethod]
        public void TestAddPicture_success()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            IWebElement addPictureBtn = webDriver.FindElement(By.XPath("//nav/button[text()='圖片']"));
            IWebElement pictureCountLabel = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//form[@id='form0']/div[3]/label")));
            
            int currentPictureCounts = Convert.ToInt32(pictureCountLabel.Text);
        
            //addPictureBtn.Click();  In this case, it don't need to do that

            IWebElement addFile = wait.Until(driver => driver.FindElement(By.XPath("//div[@id='fileupload_container']/div/input")));
            addFile.SendKeys(@"C:\Users\Public\Pictures\Sample Pictures\Chrysanthemum_2.jpg"); //tricky, tricky

            Thread.Sleep(100); // wait the element refresh value
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//form[@id='form0']/div[3]/label")));
            int additionCounts = Convert.ToInt32(pictureCountLabel.Text);
            Assert.IsTrue(additionCounts == ++currentPictureCounts);
        }

        private int wordCountLogic(string content)
        {
            int wordCount = 0;

            // 計算英文、數字
            wordCount = (int)Math.Ceiling(content.Replace("\r\n", "\n").Where(c => (int)c < 128).Count() / 2d);

            // 計算中文
            wordCount += content.Where(c => (int)c > 127).Count();

            return wordCount;
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
