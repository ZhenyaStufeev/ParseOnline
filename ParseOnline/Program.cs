using AppSettings;
using CubixParserModule.Entities;
using CubixParserModule.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrseOnline
{
    class Program
    {
        static async Task Main(string[] args)
        {

            bool all_servers = true;
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start(); //запуск
            string path_save = @"D:\TeamOnline.txt";
            string path_team = @"D:\TeamCubix.txt";
            List<string> users = new();
            List<string> list_lines = new List<string>();

            Settings settings = new Settings(Directory.GetCurrentDirectory(), "appsettings.json");
            CubixParseService cubixParse = new CubixParseService(settings);
            List<string> server_ids = new List<string>() { "14" };

            var ServersTeamInfo = cubixParse.GetServerTeamInfo(server_ids, all_servers);
            List<ProfilePlayerInfo> profile_players = new List<ProfilePlayerInfo>();

            foreach (ServerTeamInfo teamInfo in ServersTeamInfo)
            {

                list_lines.Add($"ServerName: {teamInfo.ServerName}\nServerId: {teamInfo.ServerId}\nUsers:");
                Console.WriteLine($"ServerName: {teamInfo.ServerName}\nServerId: {teamInfo.ServerId}\nUsers:");
                foreach (var user in teamInfo.UserInfo)
                {
                    if (user.GroupId == "99") //ignore 'Строитель' group
                        continue;

                    list_lines.Add($"\tUserName: {user.UserName}\n\tGroupName: {user.GroupName}\n\tGroupId: {user.GroupId}");
                    Console.WriteLine($"\tUserName: {user.UserName}\n\tGroupName: {user.GroupName}\n\tGroupId: {user.GroupId}");
                    ProfilePlayerInfo profile = profile_players.FirstOrDefault(p => p.UserName == user.UserName); //= cubixParse.GetPlayerProfile(user.UserName);
                    if (profile == null)
                    {
                        profile = cubixParse.GetPlayerProfile(user.UserName);
                        profile_players.Add(profile);
                        users.Add(profile.UserName);
                    }
                    string server_time_converted = "";
                    PlayTime play_time_user_object = profile.playTimeInfo.FirstOrDefault(p => p.server_id == teamInfo.ServerId);
                    if (play_time_user_object == null)
                        server_time_converted = ConvertMSToTimeString(0);
                    else
                        server_time_converted = ConvertMSToTimeString(play_time_user_object.time);

                    list_lines.Add($"\tOnline: {server_time_converted}\n");
                    Console.WriteLine($"\tOnline: {server_time_converted}\n");
                }
            }

            myStopwatch.Stop();
            Console.WriteLine($"\n\nRequest Count: {cubixParse.count_requests}");
            Console.WriteLine("To do time: " + ConvertMSToTimeString(myStopwatch.ElapsedMilliseconds));
            Console.WriteLine("To do time for requests: " + ConvertMSToTimeString(cubixParse.ms_requests));
            Console.WriteLine("Load config file: " + ConvertMSToTimeString(settings.time_settings));
            File.WriteAllLines(path_save, list_lines);
            File.WriteAllLines(path_team, users);
        }

        public static string ConvertMSToTimeString(long ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            string answer = string.Format("{0:D2}ч. {1:D2}м. {2:D2} с. {3:D2} мс.",
                            (int)t.TotalHours,
                            t.Minutes,
                            t.Seconds, t.Milliseconds);
            return answer;
        }
    }
}

