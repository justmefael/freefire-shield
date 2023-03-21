using KeyAuth;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace freefire_anti_cheat
{
    public class Program
    {
        public static int screenshotCount = 0;
        public static string mediatorID;

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static Thread Print = new Thread(CaptureMyScreen) { IsBackground = true };
        static Thread AntCrackThread = new Thread(AntCrack) { IsBackground = true };
        static Thread AntCrackThread2 = new Thread(AntCrack) { IsBackground = true };
        public static api KeyAuthApp = new api(
            name: "ffanticheat",
            ownerid: "EiCUv7DQTM",
            secret: "2cae7b011e5edff7fba911a91ed63b79f53efc5230e1b752c5a0a86ccb5cd924",
            version: "1.0"
        );


        static Random rnd = new Random();
        static int numID = rnd.Next();

        static void Main(string[] args)
        {
            Console.SetWindowSize(90, 25);
            Console.SetBufferSize(90, 25);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);

            //AntCrackThread.Start();
            WebClient downloadBlackListHWID = new WebClient();
            string getBlacklistHWID = downloadBlackListHWID.DownloadString(config.pastebin_link_blacklist);

            KeyAuthApp.init();
            Console.Title = "Free Fire - Shield | By: fael#2081";
            Console.Clear();

            while (true)
            {
                logo();

                Console.Write("                     PC NAME: ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Environment.UserName);
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write("   RANDOM ID: ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(numID);

                Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write("\n\n\n\n  Enter the admin id ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                mediatorID = Console.ReadLine();

                KeyAuthApp.login(mediatorID, "1");
                if (KeyAuthApp.response.success)
                {
                    //AntCrackThread.Suspend();
                    break;
                }
            }

            if (getBlacklistHWID.Contains(WindowsIdentity.GetCurrent().User.Value))
            {
                webhook.UserBanned(numID);
                error("you are banned from the shield");
            }

            if (Directory.Exists(config.save_capture))
            {
                foreach (var fPath in Directory.GetFiles(config.save_capture))
                {
                    File.Delete(fPath);
                }
            }

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            DateTime localdate = DateTime.Now;
            config.started = localdate;

            Console.Clear();

            Print.Start();
            Console.ReadKey();

            Print.Suspend();
            Thread.Sleep(100);

            DateTime localdate2 = DateTime.Now;
            config.finalized = localdate2;

            Console.Clear();
            logo();

            Console.Write("                     PC NAME: ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(Environment.UserName);
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("   RANDOM ID: ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(numID);

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("\n\n\n\n  Shield finishing... ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(screenshotCount);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" prints were taken.");

            var zipFile = config.save_capture + @"\captures.zip";
            var files = Directory.GetFiles(config.save_capture);

            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var fPath in files)
                {
                    archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                }
            }

            webhook.SendInformation(CreateDownloadLink(config.save_capture + @"\captures.zip"), numID);
        }

        static void logo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("               ███████╗██████╗░███████╗███████╗  ███████╗██╗██████╗░███████╗");
            Console.WriteLine("               ██╔════╝██╔══██╗██╔════╝██╔════╝  ██╔════╝██║██╔══██╗██╔════╝");
            Console.WriteLine("               █████╗░░██████╔╝█████╗░░█████╗░░  █████╗░░██║██████╔╝█████╗░░");
            Console.WriteLine("               ██╔══╝░░██╔══██╗██╔══╝░░██╔══╝░░  ██╔══╝░░██║██╔══██╗██╔══╝░░");
            Console.WriteLine("               ██║░░░░░██║░░██║███████╗███████╗  ██║░░░░░██║██║░░██║███████╗");
            Console.WriteLine("               ╚═╝░░░░░╚═╝░░╚═╝╚══════╝╚══════╝  ╚═╝░░░░░╚═╝╚═╝░░╚═╝╚══════╝");
            Console.WriteLine("");
            Console.WriteLine("                       ░██████╗██╗░░██╗██╗███████╗██╗░░░░░██████╗░");
            Console.WriteLine("                       ██╔════╝██║░░██║██║██╔════╝██║░░░░░██╔══██╗");
            Console.WriteLine("                       ╚█████╗░███████║██║█████╗░░██║░░░░░██║░░██║");
            Console.WriteLine("                       ░╚═══██╗██╔══██║██║██╔══╝░░██║░░░░░██║░░██║");
            Console.WriteLine("                       ██████╔╝██║░░██║██║███████╗███████╗██████╔╝");
            Console.WriteLine("                       ╚═════╝░╚═╝░░╚═╝╚═╝╚══════╝╚══════╝╚═════╝░");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void error(string message)
        {
            Process.Start(new ProcessStartInfo("cmd.exe", $"/c start cmd /C \"color b && title Error && echo {message} && timeout /t 5\"")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            });
            Environment.Exit(0);
        }
        static string CreateDownloadLink(string File)
        {
            string ReturnValue = string.Empty;
            try
            {
                using (WebClient Client = new WebClient())
                {
                    byte[] Response = Client.UploadFile("https://api.anonfiles.com/upload", File); //Edited (Working*)
                    string ResponseBody = Encoding.ASCII.GetString(Response);
                    if (ResponseBody.Contains("\"error\": {"))
                    {
                        ReturnValue += "Error message: " + ResponseBody.Split('"')[7] + "\r\n";
                    }
                    else
                    {
                        ReturnValue += ResponseBody.Split('"')[15] + "\r\n";
                    }
                }
            }
            catch (Exception Exception)
            {
                ReturnValue += "Exception Message:\r\n" + Exception.Message + "\r\n";
            }
            return ReturnValue;
        }

        static void AntCrack()
        {
            while (true)
            {
                antcrack.AntiDebug();
                antcrack.Sandboxie();
                antcrack.DetectVM();
                antcrack.Emulation();
                antcrack.CheckProcess();
                Thread.Sleep(1000);
            }
        }
        static void CaptureMyScreen()
        {
            //AntCrackThread2.Start();
            while (true)
            {
                try
                {
                    screenshotCount++;
                    DateTime localdate = DateTime.Now;

                    Console.Clear();
                    logo();
                    Console.Write("                     PC NAME: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(Environment.UserName);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write("   RANDOM ID: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(numID);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write("                     SCREEN COUNT: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(screenshotCount);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write("\n\n\n  Press any key to finish shield...");

                    Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                    Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
                    Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                    captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

                    if (Directory.Exists(config.save_capture))
                        captureBitmap.Save(config.save_capture + @"\capture+" + localdate.ToString().Replace("/", "-").Replace(":", "-").Replace(" ", "_") + "-(" + screenshotCount + ")" + ".jpg", ImageFormat.Jpeg);
                    else
                    {
                        Directory.CreateDirectory(config.save_capture);
                        captureBitmap.Save(config.save_capture + @"\capture+" + localdate.ToString().Replace("/", "-").Replace(":", "-").Replace(" ", "_") + ".jpg", ImageFormat.Jpeg);
                    }


                }
                catch { }
                Thread.Sleep(1000);
            }
        }
    }
}
