using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace Arnold_Co
{
    public class InstalledApp
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Publisher { get; set; }
        public override string ToString() => $"{Name} ({Version}) - {Publisher}";
    }

    internal class OpenAction: ActionJSON
    {
        public List<InstalledApp> allApps;
        public static List<InstalledApp> GetInstalledApplications()
        {
            List<InstalledApp> apps = new List<InstalledApp>();

            string[] registryKeys = new[]
            {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };

            // Local Machine (system-wide installs)
            foreach (var keyPath in registryKeys)
                ReadAppsFromRegistry(Registry.LocalMachine.OpenSubKey(keyPath), apps);

            // Current User (per-user installs)
            foreach (var keyPath in registryKeys)
                ReadAppsFromRegistry(Registry.CurrentUser.OpenSubKey(keyPath), apps);

            return apps;
        }

        private static void ReadAppsFromRegistry(RegistryKey baseKey, List<InstalledApp> apps)
        {
            if (baseKey == null)
                return;

            foreach (var subKeyName in baseKey.GetSubKeyNames())
            {
                using (var subKey = baseKey.OpenSubKey(subKeyName))
                {
                    string name = subKey?.GetValue("DisplayName") as string;
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        apps.Add(new InstalledApp
                        {
                            Name = name,
                            Version = subKey.GetValue("DisplayVersion") as string,
                            Publisher = subKey.GetValue("Publisher") as string
                        });
                    }
                }
            }
        }
        public override void Init()
        {
            //base.Init();
            Debug.WriteLine("Piss fart");
            allApps = GetInstalledApplications();
            Debug.WriteLine(allApps.Count);
            Debug.WriteLine(allApps.PickRandom().ToString());
        }
    }
}
