using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Arnold_Co
{
    internal class DaisyAction : ActionJSON
    {
        public override void OnCalled(string text, Dictionary<string, object> parsedParams)
        {
            base.OnCalled(text, parsedParams);
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://rootaccess.piratewit.ch/",
                UseShellExecute = true
            });
        }
    }
}
