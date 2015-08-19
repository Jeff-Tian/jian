using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

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
                Pattern = "test",
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
    }
}