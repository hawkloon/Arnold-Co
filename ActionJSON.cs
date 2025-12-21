using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace Arnold_Co
{
    public enum ActionType
    {
        Predetermined,
        Dynamic
    }
    public class ActionJSON
    {
        public class ActionParameter
        {
            public string name;          // e.g. "week"
            public string type;          // "int", "string", etc.
            public bool optional;
        }
        public string name;
        public bool useJsonResponses;
        public Dictionary<string, string[]> speakerResponses;
        public string[] keywords;
        public ActionType actionType;

        public bool useParameters;
        public ActionParameter[] parameters;

        public virtual void Init()
        {
            Debug.WriteLine("Cum");
            WriteTestJSON();
        }

        public virtual void OnCalled(string text, Dictionary<string, object> parsedParams)
        {
            if (useJsonResponses && speakerResponses != null)
            {
                if (speakerResponses.ContainsKey(Program.activePersona.name))
                {
                    var respArray = speakerResponses[Program.activePersona.name];
                    string response = respArray.PickRandom();
                    Program.activePersona.Speak(response);
                }
                else
                {
                    var respArray = speakerResponses["Default"];
                    string response = respArray.PickRandom();
                    Program.activePersona.Speak(response);
                }
            }
        }       

        public static void WriteTestJSON()
        {
            var test = new ActionJSON();
            test.name = "Test";
            test.useJsonResponses = true;
            test.speakerResponses = new Dictionary<string, string[]>();
            test.speakerResponses.Add("Speaker1", new string[] { "Hello there!", "How are you?" });
            test.keywords = new string[] { "hello", "hi", "greetings" };
            test.actionType = ActionType.Predetermined;
            test.parameters = new ActionParameter[]
            {
                new ActionParameter() { name = "param1", type = "string", optional = false },
                new ActionParameter() { name = "param2", type = "int", optional = true }
            };

            var serial = JsonConvert.SerializeObject(test, Program.serializerSettings);
            File.WriteAllText("Action_Test.json", serial);
        }
    }
}
