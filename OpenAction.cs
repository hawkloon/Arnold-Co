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
        
    }
}
