using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubixParserModule.Entities
{
    public class ServerTeamInfo
    {
        public ServerTeamInfo()
        {
            UserInfo = new List<TeamUserInfo>();
        }
        public string ServerId { get; set; }
        public string ServerName { get; set; }
        public IList<TeamUserInfo> UserInfo { get; set; }
    }
}
