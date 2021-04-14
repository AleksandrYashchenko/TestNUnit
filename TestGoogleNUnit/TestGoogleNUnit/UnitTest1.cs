using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, lengthLine)
                .Select(s => s[randomLine.Next(s.Length)]).ToArray());
        }

        [Test]
        public void Registration()
        {
            var registrationButton = driver
                .FindElement(By.XPath("//div[@class='ek-grid__item']/a[text()='зарегистрироваться']"));
            registrationButton.Click();

            var nameRegistration = driver
                .FindElement(By.XPath("//div[@class='lp-vertical-form__field']/input[@data-qaid='name']"));
            var emailRegistration = driver
                .FindElement(By.XPath("//div[@class='lp-vertical-form__field']/input[@data-qaid='email']"));
            var passwordRegistration = driver
                .FindElement(By.XPath("//div[@class='lp-vertical-form__field']/input[@data-qaid='password']"));

            string name = RandomString();
            nameRegistration.SendKeys(name);

            string email = RandomString();
            emailRegistration.SendKeys($"{email}@gmail.com");

            string password = RandomString();
            passwordRegistration.SendKeys(password);

            var buttonLogin = driver
                .FindElement(By.XPath("//button[@data-qaid='submit']"));
            buttonLogin.Click();

            // Thread.Sleep(10 * 1000); 
            Thread.Sleep(10);
        }
    }
}