using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettings.Entitites
{
    public class TeamSettings
    {
        public string teamPath { get; set; }
        public string key_servername { get; set; }
        public string key_serverId { get; set; }
        public string key_playerName { get; set; }
        public string key_groupName { get; set; }
        public string key_groupId { get; set; }
        public string key_team_section { get; set; }
        public string key_users_section { get; set; }
        public IList<string> DEBUG_server_Ids { get; set; }
    }
}
