using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace Jian
{
    public static class WebDriverWrapper
    {
        private static IWebDriver _driver;
        public static IWebDriver GetDriver()
        {
            if (_driver != null) return _driver;

            if (File.Exists("chromedriver.exe"))
            {
                return _driver = new ChromeDriver();
            }

            if (File.Exists("IEDriverServer_64.exe"))
            {
                return _driver = new InternetExplorerDriver();
            }

            if (File.Exists("IEDriverServer.exe"))
            {
                return _driver = new InternetExplorerDriver();
            }

            return null;
        }

        public static void Close()
        {
            if (_driver == null) return;

            _driver.Close();
            _driver.Quit();
            _driver.Dispose();
            _driver = null;
        }
    }
}
