using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubixParserModule.Entities
{
    public class ProfilePlayerInfo
    {
        public ProfilePlayerInfo()
        {
            playTimeInfo = new List<PlayTime>();
        }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public IList<PlayTime> playTimeInfo { get; set; }
    }
}
