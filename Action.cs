using System;
using System.Collections.Generic;
using System.Text;

namespace Arnold_Co
{
    public class Action
    {
        public string name;

        public bool useJsonResponses;
        public Dictionary<string, string[]> responses;

        public virtual void Init()
        {

        }

        public virtual void OnCalled()
        {
            if (useJsonResponses)
            {
                if (responses[Program.activePersona.name] != null)
                {
                    var respArray = responses[Program.activePersona.name];
                    string response = respArray.PickRandom();
                    Program.activePersona.Speak(response);
                }
            }
        }
        public Action(ActionJSON json)
        {
            this.name = json.name;
            this.useJsonResponses = json.useJsonResponses;
            this.responses = json.speakerResponses;

            Init();
        }
    }
}
