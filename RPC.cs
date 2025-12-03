using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Arnold_Co
{
    internal static class RPC
    {
        public static DiscordRpcClient client;

        public static void SetUp()
        {
            client = new DiscordRpcClient("1394616222078861342");

            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            client.OnReady += (sender, e) =>
            {
                Debug.WriteLine("Connected to discord as " + e.User.Username);
            };
            client.Initialize();

            UpdatePresence("WHERE ARE MY LEGS, DOC", "PLEASE GOD, IT BURNS OH SO BAD");
        }
        static readonly Assets mainAssets = new Assets()
        {
            LargeImageKey = "image_key"
        };

        public static void UpdatePresence(string details, string state)
        {
            client.SetPresence(new RichPresence()
            {
                Details = details,
                State = state,
                Assets = mainAssets
            });
        }
    }
}
