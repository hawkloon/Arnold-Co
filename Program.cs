using Google.Cloud.TextToSpeech.V1;
using KokoroSharp;
using KokoroSharp.Core;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using static Google.Rpc.Context.AttributeContext.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Arnold_Co
{
    internal static class Program
    {
        public static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Objects
        };
        static Form1 form;
        private static string personaID = "Arnold";

        public static Persona activePersona;

        public static VoiceRecognition recognizer;
        public static List<string> allPersonaIDs;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            if (args.Contains("--alarm"))
            {
                Application.Run(new Form2());
                return;
            }

            if (args.Contains("--addexe"))
            {
                var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "allowed_executables.json");
                MessageBox.Show(string.Join("\n", args));
                JsonWhitelist whitelist;
                if (Path.Exists(jsonPath))
                {
                    whitelist =  JsonConvert.DeserializeObject<JsonWhitelist>(File.ReadAllText(jsonPath), serializerSettings);

                }
                else
                {
                    whitelist = new JsonWhitelist();
                    whitelist.whitelist = new Dictionary<string, string>();
                }
                whitelist.whitelist.Add(args[1], Path.GetFileNameWithoutExtension(args[1]));
                var val = JsonConvert.SerializeObject(whitelist, serializerSettings);
                File.WriteAllText(jsonPath, val);


                return;
            }
            form = new Form1();
            SetUpContextMenu();
            RunRPC();
            GetAllPersonas();
            Debug.WriteLine("Available Personas: " + allPersonaIDs.Count);
            LoadPersona(personaID);
            recognizer = new VoiceRecognition();
            recognizer.Init();
            ActionManager.LoadAllActions();
            ActionJSON.WriteTestJSON();
            Application.Run();
        }
         
        private static void GetAllPersonas()
        {
            var dir = "Personas";
            var personaFiles = Directory.GetFiles(dir, "Persona_*.json");
            List<string> personaIDs = new List<string>();
            foreach (var personaFile in personaFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(personaFile);
                var id = fileName.Replace("Persona_", "");
                personaIDs.Add(id);
            }
            allPersonaIDs = personaIDs;
        }

        public static void LoadPersona(string id)
        {
            var dir = $"Personas\\Persona_{id}.json";
            if (!Path.Exists(dir))
            {
                Debug.WriteLine("Invalid Persona");
                return;
            }
            activePersona = null;
            var personaFile = File.ReadAllText(dir);
            var json = JsonConvert.DeserializeObject<PersonaJSON>(personaFile, serializerSettings);
            Persona persona = new Persona(json);

            activePersona = persona;
        }
        public static void RunRPC() => RPC.SetUp();
        private static void SetUpContextMenu()
        {
            NotifyIcon notifyIcon = new NotifyIcon
            {
                Icon = form.arnoldIcon.Icon,
                ContextMenuStrip = form.trayContextMenu,
                Text = "Arnold & Co",
                Visible = true
            };
        }
    }
}