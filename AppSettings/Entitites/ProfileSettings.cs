using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettings.Entitites
{
    public class ProfileSettings
    {
        public string profilePath { get; set; }
        public IList<string> profileParams { get; set; }
        public string key_playtime_section { get; set; }
        public string key_servername { get; set; }
        public string key_time { get; set; }
        public string key_username { get; set; }
        public string key_realname { get; set; }
        public string key_server_id { get; set; }
    }
}
