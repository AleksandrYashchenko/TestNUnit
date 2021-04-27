using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestGoogleNUnit
{
    public class BaseTest
    {
        public IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(45);
            driver.Navigate().GoToUrl("https://prom.ua/");
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
        }
    }

    public class CheckRegistration : BaseTest
    {
        public Random randomLine = new Random();
        int lengthLine = 6;
        public string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, lengthLine)
                .Select(s => s[randomLine.Next(s.Length)]).ToArray());
        }

        [Test]
        public void Registration()
        {
            IWebElement registrationButton = driver
                .FindElement(By.XPath("//div[@class='ek-grid__item']/a[text()='зарегистрироваться']"));
            registrationButton.Click();

            IWebElement nameRegistration = driver
                .FindElement(By.XPath("//div[@class='lp-vertical-form__field']/input[@data-qaid='name']"));
            IWebElement emailRegistration = driver
                .FindElement(By.XPath("//div[@class='lp-vertical-form__field']/input[@data-qaid='email']"));
            IWebElement passwordRegistration = driver
                .FindElement(By.XPath("//div[@class='lp-vertical-form__field']/input[@data-qaid='password']"));

            string name = RandomString();
            nameRegistration.SendKeys(name);

            string email = RandomString();
            emailRegistration.SendKeys($"{email}@gmail.com");

            string password = RandomString();
            passwordRegistration.SendKeys(password);

            IWebElement buttonLogin = driver
                .FindElement(By.XPath("//button[@data-qaid='submit']"));
            buttonLogin.Click();

            IWebElement transitionEmail = driver
                .FindElement(By.XPath("//div[@class='b-swipe-tabs']/a[@data-qaid='credentials_settings_tab']"));
            transitionEmail.Click();

            //Thread.Sleep(10 * 1000); 
            Thread.Sleep(10);

            var checkedEmail = driver
                .FindElement(By.XPath("//div[@class='b-form-unit']/input")).GetAttribute("value");
            Assert.AreEqual(email+"@gmail.com", checkedEmail, "Email doesn't match the one entered during registration");
        }

        public class SearchTest : BaseTest
        {
            [Test]
            public void Search()
            {
                IWebElement searchLine = driver
                    .FindElement(By.XPath("//input[@name='search_term']"));
                searchLine.Click();

                string searchKey = "Велосипед";
                searchLine.SendKeys(searchKey);

                IWebElement searchButton = driver
                    .FindElement(By.XPath("//button[@type='submit']"));
                searchButton.Click();

                // TODO: В дальнейшем нужно исправить Thread.Sleep на Wait.
                Thread.Sleep(10000); 
                IReadOnlyCollection<IWebElement> productsGallary = driver.
                    FindElements(By.XPath(".//span[contains(@class, 'ek-text_wrap_two-line')]"));

                foreach (IWebElement product in productsGallary)
                {
                    // Переменная сохраняющая значение которое храниться в title.
                    string titleValue = product.Text;

                    // Проверка, присутсвует ли слово велосипед в элементе коллекции.
                    bool isSearchKeyPresentInTitle = titleValue.Contains(searchKey, StringComparison.InvariantCultureIgnoreCase);

                    Assert.IsTrue(isSearchKeyPresentInTitle, "Test failed, search is not working correctly");
                }
            }
        }
    }
}