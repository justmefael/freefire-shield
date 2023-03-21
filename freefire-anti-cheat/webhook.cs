
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Principal;

namespace freefire_anti_cheat
{
    public class webhook
    {
        public static void SendInformation(string link, int rndID)
        {
            string IP = new WebClient().DownloadString("https://ipecho.net/plain");

            WebRequest wr = (HttpWebRequest)WebRequest.Create(config.webhook);

            wr.ContentType = "application/json";

            wr.Method = "POST";

            using (var sw = new StreamWriter(wr.GetRequestStream()))
            {

                string json = JsonConvert.SerializeObject(new
                {
                    username = "Free Fire Shield - BY: fael#2081",
                    embeds = new[]
                    {
                        new {
                            description =
                            "**Mediator ID / Random ID: **```" + Program.mediatorID + "・" + rndID + "```\n" +
                            "**Screenshot Upload: **```" + link + "```\n" +
                            "**PC NAME / IP: **```" + Environment.UserName + "・" + IP + "```\n" +
                            "**Start Time / Stop Time: **```" + config.started + "・" + config.finalized + "```\n" +
                            "**HWID: **```" + WindowsIdentity.GetCurrent().User.Value + "```" + "\n",
                            title = $"Free Fire Shield - Finalized",
                            color = "14698581",

                            footer = new {
                                icon_url = "",
                                text = "Free Fire - Shield ・ Screenshot Count: " + Program.screenshotCount,
                            },
                        }
                    }
                });

                sw.Write(json);
            }
            var response = (HttpWebResponse)wr.GetResponse();
        }

        public static void UserBanned(int rndID)
        {
            string IP = new WebClient().DownloadString("https://ipecho.net/plain");

            WebRequest wr = (HttpWebRequest)WebRequest.Create(config.webhookbanned);

            wr.ContentType = "application/json";

            wr.Method = "POST";

            using (var sw = new StreamWriter(wr.GetRequestStream()))
            {

                string json = JsonConvert.SerializeObject(new
                {
                    username = "Free Fire Shield - BY: fael#2081",
                    embeds = new[]
                    {
                        new {
                            description =
                            "**Mediator ID / Random ID: **```" + Program.mediatorID + "・" + rndID + "```\n" +
                            "**PC NAME / IP: **```" + Environment.UserName + "・" + IP + "```\n" +
                            "**HWID: **```" + WindowsIdentity.GetCurrent().User.Value + "```" + "\n",
                            title = $"Free Fire Shield - User banned",
                            color = "14698581",

                            footer = new {
                                icon_url = "",
                                text = "Free Fire - Shield",
                            },
                        }
                    }
                });

                sw.Write(json);
            }
            var response = (HttpWebResponse)wr.GetResponse();
        }
    }
}
