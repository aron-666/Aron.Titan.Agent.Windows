using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aron.Titan.Agent.Windows.Config
{
    public class Config
    {

        /// <summary>
        /// 資料目錄
        /// </summary>
        public string? DataDir { get; set; }

        /// <summary>
        /// 身分碼
        /// </summary>
        public string? Key { get; set; }

        public void Save()
        {
            string json = File.ReadAllText("appsettings.json");
            JObject obj = JObject.Parse(json);
            obj["Config"]["DataDir"] = DataDir;
            obj["Config"]["Key"] = Key;
            File.WriteAllText("appsettings.json", obj.ToString());
        }

    }
}
