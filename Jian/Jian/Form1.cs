using System;
using System.Collections.Generic;
using System.Data;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadSetting();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JianWrapper.GetJian().FromDataTable(dg.DataSource as DataTable, false).SaveToFile();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadSetting();
        }

        private void ReloadSetting()
        {
            dg.DataSource = JianWrapper.LoadFromFile().ToDataTable();
            dg.Refresh();
        }

        private void 分析AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var page in JianWrapper.GetJian().Pages)
            {
                var url = page.Url;

                foreach (var interest in page.Interests.Keys)
                {
                    var xpath = page.Interests[interest].Pattern;

                    WebDriverWrapper.GetDriver().Navigate().GoToUrl(url);

                    var wait = new WebDriverWait(WebDriverWrapper.GetDriver(), TimeSpan.FromSeconds(120));

                    try
                    {
                        wait.Until(d => FindElement(d, xpath));
                        var element = FindElement(WebDriverWrapper.GetDriver(), xpath);
                        page.Interests[interest].Value = element.Text;
                    }
                    catch (Exception)
                    {
                        page.Interests[interest].Value = "查找失败。(请重试或者在设置中修改特征字符)";
                    }
                }
            }

            WebDriverWrapper.Close();

            JianWrapper.GetJian().SaveToFile();
            ReloadSetting();

            MessageBox.Show("分析完毕。");
        }

        private static IWebElement FindElement(IWebDriver driver, string xpath)
        {
            return driver.FindElement(By.XPath(xpath));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            WebDriverWrapper.Close();
        }

        private void 设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSettings().ShowDialog(this);
        }

        private void 关于BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("当前目录：" + Directory.GetCurrentDirectory());
        }
    }
}