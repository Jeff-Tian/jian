using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Jian
{
    public class JianWrapper
    {
        private static JianEnt _jianEnt;

        public static JianEnt GetJian()
        {
            return _jianEnt ?? (_jianEnt = new JianEnt());
        }

        public static JianEnt LoadFromFile(string path = "jian.json")
        {
            _jianEnt = JianEnt.LoadFromFile(path);

            return GetJian();
        }
    }


    public class JianEnt
    {
        public List<Page> Pages { get; set; }

        public static JianEnt LoadFromFile(string path = "jian.json")
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<JianEnt>(json);
        }

        public void SaveToFile(string path = "jian.json")
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText("jian.json", json);
        }

        public JianEnt FromDataTable(DataTable dt, bool settingMode = true)
        {
            var pages = new List<Page>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var page = new Page
                {
                    Name = dt.Rows[i]["名字"].ToString(),
                    Url = dt.Rows[i]["网址"].ToString(),
                    Interests = new Dictionary<string, PatternValuePair>()
                };

                if (settingMode)
                {
                    for (var j = 2; j < dt.Rows[i].ItemArray.Length; j++)
                    {
                        var col = dt.Rows[i].ItemArray[j];
                        page.Interests.Add(dt.Columns[j].ColumnName, new PatternValuePair()
                        {
                            Pattern = col.ToString(),
                            Value = ""
                        });
                    }

                    var oldPage = this.Pages.FirstOrDefault(p => p.Url.Trim(' ', '/').Equals(page.Url.Trim(' ', '/')));

                    if (oldPage != null)
                    {
                        foreach (
                            var interest in
                                page.Interests.Where(interest => oldPage.Interests.ContainsKey(interest.Key)))
                        {
                            page.Interests[interest.Key].Value = oldPage.Interests[interest.Key].Value;
                        }
                    }
                }
                else
                {
                    for (var j = 2; j < dt.Rows[i].ItemArray.Length; j++)
                    {
                        var col = dt.Rows[i].ItemArray[j];
                        page.Interests.Add(dt.Columns[j].ColumnName, new PatternValuePair()
                        {
                            Pattern = "",
                            Value = col.ToString()
                        });
                    }

                    var oldPage = Pages.FirstOrDefault(p => p.Url.Trim(' ', '/').Equals(page.Url.Trim(' ', '/')));

                    if (oldPage != null)
                    {
                        foreach (
                            var interest in
                                page.Interests.Where(interest => oldPage.Interests.ContainsKey(interest.Key)))
                        {
                            page.Interests[interest.Key].Pattern = oldPage.Interests[interest.Key].Pattern;
                        }
                    } 
                }

                pages.Add(page);
            }

            Pages = pages;

            return this;
        }

        public DataTable ToDataTable(bool settingMode = false)
        {
            var interests = new List<string>();

            foreach (var interest in Pages.SelectMany(page => page.Interests.Where(interest => !interests.Contains(interest.Key))))
            {
                interests.Add(interest.Key);
            }

            var dt = new DataTable();
            dt.Columns.Add("名字");
            dt.Columns.Add("网址");

            foreach (var interest in interests)
            {
                dt.Columns.Add(interest);
            }

            foreach (var page in Pages)
            {
                var cells = new List<object> {page.Name, page.Url};

                cells.AddRange(!settingMode
                    ? interests.Select(interest => page.Interests[interest].Value)
                    : interests.Select(interest => page.Interests[interest].Pattern));

                dt.Rows.Add(cells.ToArray());
            }

            return dt;
        }
    }

    public class Page
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public Dictionary<string, PatternValuePair> Interests { get; set; }
    }

    public class PatternValuePair
    {
        public string Pattern { get; set; }
        public string Value { get; set; }
    }
}