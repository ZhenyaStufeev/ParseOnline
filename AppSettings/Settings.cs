using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;

using Newtonsoft.Json.Linq;
using AppSettings.Entitites;

namespace AppSettings
{
    public class Settings : ISettings
    {
        private ConfigurationBuilder builder { get; set; }
        private IConfigurationRoot config { get; set; }
        public string baseURL { private set; get; }
        public ProfileSettings profile { get; private set; }
        public TeamSettings team { get; private set; }
        public bool DebugEnabled { get; private set; }
        public long time_settings = 0;
        public Settings(string basePath, string jsonFullFileName)
        {
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start(); //запуск
            builder = new ConfigurationBuilder();
            builder.SetBasePath(basePath);
            builder.AddJsonFile(jsonFullFileName);
            config = builder.Build();
            this.InnitAppSettings();
            myStopwatch.Stop();
            time_settings = myStopwatch.ElapsedMilliseconds;
        }

        private void InnitAppSettings()
        {
            baseURL = config.GetSection("baseURL").Value;
            DebugEnabled = bool.Parse(config.GetSection("debug").Value);
            profile = GetProfileSettings();
            team = GetTeamSettings();
        }
        private ProfileSettings GetProfileSettings()
        {
            ProfileSettings profile = new ProfileSettings();
            var profile_section = config.GetSection("profile");
            profile.profilePath = profile_section.GetSection("profilePath").Value;
            profile.profileParams = profile_section.GetSection("profileParams").GetChildren().Select(p => p.Value).ToList();
            profile.key_playtime_section = profile_section.GetSection("key_playtime_section").Value;
            profile.key_servername = profile_section.GetSection("key_servername").Value;
            profile.key_time = profile_section.GetSection("key_time").Value;
            profile.key_realname = profile_section.GetSection("key_realname").Value;
            profile.key_username = profile_section.GetSection("key_username").Value;
            profile.key_server_id = profile_section.GetSection("key_serverId").Value;
            return profile;
        }

        private TeamSettings GetTeamSettings()
        {
            TeamSettings team = new TeamSettings();
            var team_section = config.GetSection("team");
            team.teamPath = team_section.GetSection("teamPath").Value;
            team.key_servername = team_section.GetSection("key_servername").Value;
            team.key_serverId = team_section.GetSection("key_serverId").Value;
            team.key_playerName = team_section.GetSection("key_playerName").Value;
            team.key_groupName = team_section.GetSection("key_groupName").Value;
            team.key_groupId = team_section.GetSection("key_groupId").Value;
            team.key_team_section = team_section.GetSection("key_team_section").Value;
            team.key_users_section = team_section.GetSection("key_users_section").Value;
            if (DebugEnabled)
                team.DEBUG_server_Ids = team_section.GetSection("DEBUG_server_ids").GetChildren().Select(p => p.Value).ToList();
            return team;
        }
    }
}
