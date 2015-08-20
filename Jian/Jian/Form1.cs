using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Jian
{
    public partial class Form1 : Form
    {
        private JianEnt _jianEnt;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _jianEnt = new JianEnt
            {
                Pages = new List<Page>
                {
                    new Page()
                    {
                        Name = "天猫",
                        Url =
                            "https://detail.tmall.com/item.htm?spm=a1z10.3-b.w4011-2531410378.267.NkV9Lq&id=19759056799&rn=bd9c1246aa3a2d0a170b45100d22b3eb&abbucket=13",
                        Interests = new Dictionary<string, PatternValuePair>()
                    }
                }
            };

            _jianEnt.Pages[0].Interests.Add("促销价", new PatternValuePair()
            {
                Pattern = @"//div[@class='tm-promo-price']/span[@class='tm-price']",
                Value = "test"
            });

            dg.DataSource = _jianEnt.Pages;
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var json = JsonConvert.SerializeObject(_jianEnt);
            File.WriteAllText("jian.json", json);
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadSetting();
        }

        private void ReloadSetting()
        {
            var json = File.ReadAllText("jian.json");
            _jianEnt = JsonConvert.DeserializeObject<JianEnt>(json);

            dg.DataSource = _jianEnt.Pages;
            dg.Refresh();
        }

        private void 分析AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var page in _jianEnt.Pages)
            {
                var url = page.Url;

                foreach (var interest in page.Interests.Keys)
                {
                    var xpath = page.Interests[interest].Pattern;

                    WebDriverWrapper.GetDriver().Navigate().GoToUrl(url);

                    var wait = new WebDriverWait(WebDriverWrapper.GetDriver(), TimeSpan.FromSeconds(120));

                    wait.Until(d => FindElement(d, xpath));
                    var element = FindElement(WebDriverWrapper.GetDriver(), xpath);
                    page.Interests[interest].Value = element.Text;
                }
            }

            WebDriverWrapper.Close();
        }

        private static IWebElement FindElement(IWebDriver driver, string xpath)
        {
            return driver.FindElement(By.XPath(xpath));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            WebDriverWrapper.Close();
        }
    }
}