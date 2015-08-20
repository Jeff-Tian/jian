using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Jian
{
    public static class WebDriverWrapper
    {
        private static IWebDriver _driver;
        public static IWebDriver GetDriver()
        {
            return _driver ?? (_driver = new ChromeDriver());
        }

        public static void Close()
        {
            if (_driver == null) return;

            _driver.Close();
            _driver.Quit();
        }
    }
}
