using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Google.Api.Gax.Grpc.Gcp.AffinityConfig.Types;

namespace Arnold_Co
{
    internal class OutputAction : ActionJSON
    {
        internal static string speakerID = "Speakers (High Definition Audio Device)";
        internal static string headphoneID = "Speakers (Focusrite USB Audio)";


        internal static void ChangeOutput(string deviceId)
        {
            var controller = new CoreAudioController();

            var device = controller.GetPlaybackDevices()
                .FirstOrDefault(d => d.FullName.Contains(deviceId));
            if (device == null) Debug.WriteLine("I am lying");
            device?.SetAsDefault();

        }


        public string[] devices = ["output speakers", "output headphone"];
        public override void OnCalled(string text, string parsedParams)
        {
            base.OnCalled(text, parsedParams);
            Program.activePersona.Speak("Give me a moment you gimp");
            string phrase = "";
            foreach (var keyword in devices)
            {
                if (text.ToLower().Contains(keyword))
                {
                    phrase = keyword;
                    break;
                }
            }

            Debug.WriteLine(phrase);

            switch (phrase)
            {
                case "output speakers":
                    ChangeOutput(speakerID);
                    break;
                case "output headphone":
                    ChangeOutput(headphoneID);
                    break;

            }
        }

    }
}
