using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace SpectatorList
{
public class EventHandler
    {
        private const string CoroutineTag = "Spectator-List"; 

        private static Config Config => SpectatorList.Instance.Config;

        public EventHandler()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        ~EventHandler()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Timing.KillCoroutines(CoroutineTag);
        }
        
        private void OnRoundStarted() => Timing.RunCoroutine(DoList().CancelWith(Server.Host.GameObject), CoroutineTag);

        private IEnumerator<float> DoList()
        {
            while (true)
            {
                foreach (var player in Player.List)
                {
                    if (player.IsDead || Config.HiddenFor.Contains(player.Role.Team)) continue;

                    var count = player.CurrentSpectatingPlayers.Count(p => p.Role != RoleTypeId.Overwatch);

                    var sb = new StringBuilder();
                    sb.AppendLine(count == 0
                        ? Config.NoSpectators
                        : Config.Spectators.Replace("%amount%", count.ToString()));

                    var staff = new List<string>();
                    staff.Add("0owner");
                    staff.Add("0admin");
                    staff.Add("0teamleitung");
                    staff.Add("0communitymanager");
                    staff.Add("0moderator");
                    staff.Add("0senior_supporter");
                    staff.Add("0supporter");
                    staff.Add("0junior_supporter");
                    
                    foreach (var spectator in player.CurrentSpectatingPlayers.Where(p => p.Role != RoleTypeId.Overwatch))
                    {
                        var Role = "";
                        if (spectator.GroupName.Contains("owner"))
                            Role = "<color=red>" + "⌠Owner⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("admin")) 
                            Role = "<color=red>" + "⌠Admin⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("teamleitung")) 
                            Role = "<color=pumpkin>" + "⌠Teamleitung⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("communitymanager")) 
                            Role = "<color=yellow>" + "⌠Communitymanager⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("moderator")) 
                            Role = "<color=cyan>" + "⌠Moderator⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("senior_supporter")) 
                            Role = "<color=green>" + "⌠Senior Supporter⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("supporter")) 
                            Role = "<color=green>" + "⌠Supporter⌡</color> - "+ spectator.CustomName;
                        if (spectator.GroupName.Contains("junior_supporter")) 
                            Role = "<color=green>" + "⌠Junior Supporter⌡</color> - "+ spectator.CustomName;
                        
                        if (staff.Contains("0"+spectator.GroupName.Split(".")[0].Replace("0", "")))
                        {
                            sb.AppendLine(Config.PlayerDisplay.Replace("%name%", Role));
                        }
                        
                        if (spectator.UserId == "76561199146721347@steam")
                        {
                            sb.AppendLine(Config.LunaNameColor.Replace("%name%", spectator.CustomName));
                        }
                        else if (spectator.UserId != "76561199146721347@steam" && !staff.Contains("0"+spectator.GroupName.Split(".")[0].Replace("0", "")))
                        {
                            sb.AppendLine(Config.PlayerDisplay.Replace("%name%", spectator.CustomName));
                        }
                    }
                    player.ShowHint(Config.FullText.Replace("%display%", sb.ToString()), Config.RefreshRate + 0.15f);
                }

                yield return Timing.WaitForSeconds(Config.RefreshRate);
            }
        } 
    }
}