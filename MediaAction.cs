using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Arnold_Co
{
    public static class MediaKeyController
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const byte VK_MEDIA_NEXT_TRACK = 0xb0;
        private const byte VK_MEDIA_PREV_TRACK = 0xb1;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x1;
        private const uint KEYEVENTF_KEYUP = 0x2;


        public static void TogglePlayPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void NextTrack()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void PreviousTrack()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }
    internal class MediaAction : ActionJSON
    {
        public override void OnCalled(string text, Dictionary<string, object> parsedParams)
        {
            base.OnCalled(text, parsedParams);
            if(text.Contains("play") || text.Contains("pause") || text.Contains("toggle"))
            {
                MediaKeyController.TogglePlayPause();
            }
            else if (text.Contains("next"))
            {
                MediaKeyController.NextTrack();
            }
            else if (text.Contains("previous") || text.Contains("back"))
            {
                MediaKeyController.PreviousTrack();
            }
        }
    }
}
