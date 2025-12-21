using System;
using System.Collections.Generic;
using System.Text;

namespace Arnold_Co
{
    internal class TimeAction : ActionJSON
    {
        public override void OnCalled(string text, Dictionary<string, object> parsedParams)
        {
            base.OnCalled(text, parsedParams);
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            string t = time.ToString("hh:mm tt");

            Program.activePersona.Speak("Faggot. The current time is " + t);
        }
    }
}
