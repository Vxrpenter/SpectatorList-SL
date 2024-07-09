using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace SpectatorList
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("A list of teams the hints should be hidden for")]
        public List<Team> HiddenFor { get; set; } = new List<Team>();
        [Description("The Group Names of your Staff")]
        public List<string> StaffRoleNames { get; set; } = new List<string>();

        [Description("How often in seconds to refresh the hud")]
        public float RefreshRate { get; set; } = 1;
        [Description("Location and size of the HUD")]
        public string FullText { get; set; } = "<size=20><align=right>%display%</size><voffset=900> </voffset></align>";
        [Description("How the name of the player is displayed")]
        public string PlayerDisplay { get; set; } = "%name%";
        [Description("What to show if no one is spectating, leave empty to remove")]
        public string NoSpectators { get; set; } = "No one is currently spectating you.";
        [Description("The HUD when a player is spectating")]
        public string Spectators { get; set; } = "<color=#ff8c00>»👥 No Lifes«</color>";
        [Description("Just a special name input, works only for a single steamID rn")]
        public string LunaNameColor { get; set; } = "🐻💜 <color=#FF10F0>%name%</color>";
    }
}