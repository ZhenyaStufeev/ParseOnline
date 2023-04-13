using AppSettings;
using CubixParserModule.Entities;
using CubixParserModule.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CubixParserModule.Services
{
    public class CubixParseService : ICubixParseService
    {
        private ISettings settings { get; set; }
        public int count_requests = 0;
        public CubixParseService(ISettings settings) //WebClient
        {
            this.settings = settings;
        }
        public long ms_requests = 0;
        public async Task<ProfilePlayerInfo> GetPlayerProfileAsync(string user_name)
        {
            return await Task.Run(() =>
            {
                return GetPlayerProfile(user_name);
            });
        }
        public ProfilePlayerInfo GetPlayerProfile(string user_name)
        {
            string user_params = string.Join("&", settings.profile.profileParams).Replace("{user_name}", user_name);
            string requestURL = settings.baseURL + settings.profile.profilePath + "?" + user_params;
            string responce = Request(requestURL);
            JObject profile_data = JObject.Parse(responce);
            var profile_props = profile_data.Properties();
            ProfilePlayerInfo playerInfo = new ProfilePlayerInfo();
            foreach (var prop in profile_props)
            {
                if (prop.Name == settings.profile.key_realname)
                    playerInfo.RealName = prop.Value.ToString();

                if (prop.Name == settings.profile.key_username)
                    playerInfo.UserName = prop.Value.ToString();

                if (prop.Name == settings.profile.key_playtime_section)
                {
                    for (int i = 0; i < prop.Value.Count(); ++i) //Костыль, но подругому никак
                    {
                        var obj_playtime = prop.Value[i.ToString()];
                        if (obj_playtime == null)
                            continue;

                        PlayTime playTime = new PlayTime();

                        var server_name = obj_playtime[settings.profile.key_servername];
                        var time = obj_playtime[settings.profile.key_time];
                        var server_id = obj_playtime[settings.profile.key_server_id];

                        if (server_name == null || time == null || server_id == null)
                            continue;

                        playTime.server_name = server_name.ToString();
                        playTime.time = time.ToObject<long>();
                        playTime.server_id = server_id.ToString();
                        playerInfo.playTimeInfo.Add(playTime);

                    }
                }
            }
            return playerInfo;
        }


        public List<ServerTeamInfo> GetServerTeamInfo(IList<string> server_ids, bool all_servers = false)
        {
            ServerTeamInfo server = new ServerTeamInfo();
            string requestURL = settings.baseURL + settings.team.teamPath;

            string responce = Request(requestURL);
            JObject team_data = JObject.Parse(responce);
            var team_props = team_data.Properties();
            List<ServerTeamInfo> servers_teams = new List<ServerTeamInfo>();

            foreach (var prop in team_props)
            {
                if (prop.Name == settings.team.key_team_section)
                {
                    for (int i = 0; i < prop.Value.Count(); ++i)
                    {
                        var obj_team_section = prop.Value[i.ToString()];
                        var currently_server_id = obj_team_section[settings.team.key_serverId].ToString();

                        if (server_ids.Any(p => p == currently_server_id) || all_servers == true)
                        {
                            ServerTeamInfo serverTeamInfo = new ServerTeamInfo();
                            serverTeamInfo.ServerId = currently_server_id;
                            serverTeamInfo.ServerName = obj_team_section[settings.team.key_servername].ToString();
                            var obj_users_section = obj_team_section[settings.team.key_users_section];

                            for (int j = 0; j < obj_users_section.Count(); ++j)
                            {
                                var currently_user = obj_users_section[j.ToString()];
                                TeamUserInfo teamUser = new TeamUserInfo();
                                teamUser.UserName = currently_user[settings.team.key_playerName].ToString();
                                teamUser.GroupId = currently_user[settings.team.key_groupId].ToString();
                                teamUser.GroupName = currently_user[settings.team.key_groupName].ToString();
                                serverTeamInfo.UserInfo.Add(teamUser);
                            }
                            servers_teams.Add(serverTeamInfo);
                        }
                    }
                }
            }
            return servers_teams;
        }

        private string Request(string url)
        {
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start(); //запуск
            //Console.WriteLine("\n\n" + url + "\n\n");
            var result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                response.Close();
            }
            count_requests++;
            myStopwatch.Stop();
            ms_requests += myStopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}
