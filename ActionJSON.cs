using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public string name;
        public bool useJsonResponses;
        public Dictionary<string, string[]> speakerResponses;
        public string[] keywords;
        public ActionType actionType;

        public virtual void Init()
        {

        }


        public ActionJSON()
        {
            Init();
        }
        public virtual void OnCalled()
        {
            if (useJsonResponses)
            {
                if (speakerResponses[Program.activePersona.name] != null)
                {
                    var respArray = speakerResponses[Program.activePersona.name];
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

            var serial = JsonConvert.SerializeObject(test, Program.serializerSettings);
            File.WriteAllText("Action_Test.json", serial);
        }
    }
}
