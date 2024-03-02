using DotNetSelenium.Extensions;
using OpenQA.Selenium;


namespace DotNetSelenium.Pages
{
    public class LoginPage(IWebDriver driver)
    {
        //*****Prior to C#12
        //private readonly IWebDriver driver;
        //public LoginPage(IWebDriver driver)
        //{
        //    this.driver = driver;   
        //}
        //**********************************
        IWebElement LoginLink => driver.FindElement(By.Id("loginLink"));
        IWebElement UserNameTxt => driver.FindElement(By.Id("UserName"));
        IWebElement PasswordTxt => driver.FindElement(By.Id("Password"));
        IWebElement BtnLogin => driver.FindElement(By.CssSelector(".btn"));
        IWebElement LnkEmployeeDetails => driver.FindElement(By.LinkText("Employee Details"));
        IWebElement LnkManageUsers => driver.FindElement(By.LinkText("Manage Users"));
        IWebElement LnklLogoff => driver.FindElement(By.LinkText("Log off"));

        public void ClickLogin()
        {
           LoginLink.ClickElement();
        }
        public void Login(string username, string password)
        {
            UserNameTxt.EnterText(username);
            PasswordTxt.EnterText(password);
            BtnLogin.SubmitElement();
        }
        public (bool employeeDetails, bool manageUsers) IsLoggedIn()
        {
            return (LnkEmployeeDetails.Displayed, LnkManageUsers.Displayed);
        }
    }
}
