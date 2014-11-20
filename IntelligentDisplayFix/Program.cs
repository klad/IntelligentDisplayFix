using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace IntelligentDisplayFix
{
    static class Program
    {
        private const string PSKILL_EXE = @"C:\tools\sysinternals\pskill.exe";
        private const string QUICKSET_EXE = @"C:\Program Files\Dell\QuickSet\quickset.exe";

        [STAThread]
        static void Main()
        {
            EnsureApplicationsExist();

            RestartQuickSet();

            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            Application.Run();
        }

        private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if(e.Mode == PowerModes.Resume)
                RestartQuickSet();
        }

        private static void EnsureApplicationsExist()
        {
            Trace.Assert(
                File.Exists(PSKILL_EXE),
                "File not found: \{PSKILL_EXE}"
            );
            Trace.Assert(
                File.Exists(QUICKSET_EXE),
                "File not found: \{QUICKSET_EXE}"
            );
        }

        private static void KillProcessByName(string processName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = PSKILL_EXE;
            startInfo.Arguments = processName;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            var process = Process.Start(startInfo);
            process.WaitForExit();
        }

        private static void StartQucikSet()
        {
            Process.Start(QUICKSET_EXE);
        }

        private static void RestartQuickSet()
        {
            KillProcessByName("quickset");

            StartQucikSet();
        }

    }
}
