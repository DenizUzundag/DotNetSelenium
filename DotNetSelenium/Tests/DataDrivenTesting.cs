using DotNetSelenium.Model;
using DotNetSelenium.Pages;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

namespace DotNetSelenium.Tests
{
    public class DataDrivenTesting
    {
        private IWebDriver _driver;
  

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Navigate().GoToUrl("http://eaapp.somee.com/");
            _driver.Manage().Window.Maximize();
        }

        [Test]
        [Category("ddt")]
        [TestCaseSource(nameof(Login))]
        public void TestWithPOM(LoginModel loginModel)
        {
            //POM Initalization
            LoginPage loginPage = new LoginPage(_driver);

            //Act
            loginPage.ClickLogin();
            loginPage.Login(loginModel.Username, loginModel.Password);

            //Assert
            var getLoggedIn = loginPage.IsLoggedIn();
            Assert.IsTrue(getLoggedIn.employeeDetails && getLoggedIn.manageUsers);
        }




        [Test]
        [Category("ddt")]
        [TestCaseSource(nameof(Login))]
        public void TestWithPOMUsingFluentAssertion(LoginModel loginModel)
        {
            // POM initalization
            //Arrange
            LoginPage loginPage = new LoginPage(_driver);

            //Act
            loginPage.ClickLogin();
            loginPage.Login(loginModel.Username, loginModel.Password);

            //Assert
            var getLoggedIn = loginPage.IsLoggedIn();
            getLoggedIn.employeeDetails.Should().BeTrue();
            getLoggedIn.manageUsers.Should().BeTrue();
        }




        [Test]
        [Category("ddt")]
        public void TestWithPOMWithJsonData()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "login.json");
            var jsonString = File.ReadAllText(jsonFilePath);

            var loginModel = JsonSerializer.Deserialize<LoginModel>(jsonString);

            // POM initalization
            LoginPage loginPage = new LoginPage(_driver);

            //Act
            loginPage.ClickLogin();

            loginPage.Login(loginModel.Username, loginModel.Password);
            //Assert
            var getLoggedIn = loginPage.IsLoggedIn();
            Assert.IsTrue(getLoggedIn.employeeDetails && getLoggedIn.manageUsers);
        }

        public static IEnumerable<LoginModel> Login()
        {
            yield return new LoginModel()
            {
                Username = "admin",
                Password = "password"
            };
          
        }
        public static IEnumerable<LoginModel> LoginJsonDataSource()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "login.json");
            var jsonString = File.ReadAllText(jsonFilePath);

            var loginModel = JsonSerializer.Deserialize<List<LoginModel>>(jsonString);

            foreach (var loginData in loginModel)
            {
                yield return loginData;
            }
        }
        private void ReadJsonFile()
        {

            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "login.json");
            var jsonString = File.ReadAllText(jsonFilePath);

            var loginModel = JsonSerializer.Deserialize<LoginModel>(jsonString);

            Console.WriteLine($"UserName: {loginModel.Username} Password: {loginModel.Password}");
        }


        [TearDown]
        public void TearDown()
        {
            // _driver nesnesini kapatma veya temizleme işlemleri burada yapılmalıdır.
            _driver.Dispose();
        }
    }
}
