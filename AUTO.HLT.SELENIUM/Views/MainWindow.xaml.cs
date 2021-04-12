using System;
using System.IO;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AUTO.HLT.SELENIUM.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("user-data-dri="+AppDomain.CurrentDomain.BaseDirectory+ "/data");
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://www.facebook.com");
            driver.FindElement(By.Id("email")).SendKeys("bonglv@outlook.com");
            driver.FindElement(By.Id("pass")).SendKeys("Bonglvno1@");
            driver.FindElement(By.Name("login")).Click();
            var cookie = driver.Manage().Cookies.AllCookies;
            var scr = driver.PageSource;
            driver.Quit();
        }
    }
}
