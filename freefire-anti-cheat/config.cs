using System;

namespace freefire_anti_cheat
{
    public class config
    {
        public static string pastebin_link_blacklist = "https://pastebin.com/raw/Czw7GF2a"; // pastebin where constains the hwid banned
        public static string save_capture = @"C:\ffshield";

        public static string webhook = "https://discord.com/api/webhooks/1006641483610329159/Dau1AqXkU3z2ujFO0PG5Smz96kp3dWsE-OqbdjszTNqGQvXDF9DYsaH2dXXfi_QSMRWl";
        public static string webhookbanned = "https://discord.com/api/webhooks/1006892545470627840/xq0OBrgyo1m3dcMGsJWFx6kaeyJXOzbNz7jcj-XVznrdPEaS9y8kypgRgpT6LFMTZv1I";

        public static DateTime started { get; set; }
        public static DateTime finalized { get; set; }

    }
}