using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Arnold_Co
{
    internal class ActionManager
    {
        public static List<ActionJSON> actions;


        public static void LoadAllActions()
        {
            actions = new List<ActionJSON>();
            var dir = "Actions";
            var actionFiles = Directory.GetFiles(dir, "Action_*.json");
            Debug.WriteLine(actionFiles.Length);
            foreach (var actionFile in actionFiles)
            {
                ActionJSON json = JsonConvert.DeserializeObject<ActionJSON>(File.ReadAllText(actionFile), Program.serializerSettings);
                Debug.WriteLine(json.name);
                actions.Add(json);
            }

        }
    }
}
