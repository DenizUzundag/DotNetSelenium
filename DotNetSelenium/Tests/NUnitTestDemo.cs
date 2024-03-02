using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using DotNetSelenium.Driver;
using DotNetSelenium.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace DotNetSelenium.Tests
{
    [TestFixture("admin","password",DriverType.Firefox)]
  
    public class NunitTestDemo
    {
        private IWebDriver _driver;
        private readonly string userName;
        private readonly string password;
        private readonly DriverType driverType;
        private ExtentReports _extentReports;
        private ExtentTest _extentTest;
        private ExtentTest _testNode;

        public NunitTestDemo(string userName, string password, DriverType driverType)
        {
            this.userName = userName;
            this.password = password;
            this.driverType = driverType;

        }
        [SetUp] 
        public void SetUp()
        {
            SetupExtentReports();
            _driver = GetDriverType(driverType);
            _testNode= _extentTest.CreateNode("Setup and TearDown Info").Pass("Browser Launched");
            _driver.Navigate().GoToUrl("http://eaapp.somee.com/");
            _driver.Manage().Window.Maximize();
         
        }
        private IWebDriver GetDriverType(DriverType driverType)
        {
            return driverType switch
            {
                DriverType.Chrome => new ChromeDriver(),
                DriverType.Firefox => new FirefoxDriver(),
                _ =>_driver

            };
            //switch (driverType)
            //{
            //    case DriverType.Chrome:
            //        _driver = new ChromeDriver();
            //        break;
            //    case DriverType.Firefox:
            //        _driver = new FirefoxDriver();
            //        break;
            //}

            //return _driver;
        }
        private void SetupExtentReports()
        {
            _extentReports = new ExtentReports();
            var spark = new ExtentSparkReporter("TestReport.html");
            _extentReports.AttachReporter(spark);
            _extentReports.AddSystemInfo("OS", "Windows 10");
            _extentReports.AddSystemInfo("Browser", driverType.ToString());
            _extentTest = _extentReports.CreateTest("Login Test with POM").Log(Status.Info,"Extent Report Initialize");
            _extentReports.Flush();

        }

        [Test]
        [Category("smoke")]
        public void TestWithPOM()
        {
            //POM Initalization
            LoginPage loginPage = new LoginPage(_driver);
            loginPage.ClickLogin();
            _extentTest.Log(Status.Pass, "Click Login");
            loginPage.Login(userName,password);
            _extentTest.Log(Status.Pass, "Username and Password entered with login happened");
            var getLoggedIn = loginPage.IsLoggedIn();
            Assert.IsTrue(getLoggedIn.employeeDetails && getLoggedIn.manageUsers);
            _extentTest.Log(Status.Pass, "Assertion succesful");

        }

        [Test]
        [TestCase("chrome","30")]
        public void TestBrowserVersion(string browser,string version)
        {
            Console.WriteLine($"The browser is {browser} with version {version}");
        }

        [TearDown]
        public void TearDown()
        {
            // _driver nesnesini kapatma veya temizleme işlemleri burada yapılmalıdır.
            _driver.Dispose();
            _testNode.Pass("Browser quit");
            _extentReports.Flush();
        
        }


    }
}

