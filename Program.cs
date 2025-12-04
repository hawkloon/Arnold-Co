using Google.Cloud.TextToSpeech.V1;
using KokoroSharp;
using KokoroSharp.Core;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System.Diagnostics;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using static Google.Rpc.Context.AttributeContext.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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


        public static List<string> allPersonaIDs;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            form = new Form1();
            SetUpContextMenu();
            RunRPC();
            GetAllPersonas();
            Debug.WriteLine("Available Personas: " + allPersonaIDs.Count);
            LoadPersona(personaID);
            var recognizer = new VoiceRecognition();
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
            activePersona = null;
            var dir = $"Personas\\Persona_{id}.json";
            var personaFile = File.ReadAllText(dir);
            var json = JsonConvert.DeserializeObject<PersonaJSON>(personaFile, serializerSettings);
            Persona persona = new Persona(json);

            activePersona = persona;
        }
        public static void RunRPC() => RPC.SetUp();
        private static void SetUpContextMenu()
        {
            NotifyIcon notifyIcoin = new NotifyIcon
            {
                Icon = form.arnoldIcon.Icon,
                ContextMenuStrip = form.trayContextMenu,
                Text = "Arnold & Co",
                Visible = true
            };
        }
    }
}