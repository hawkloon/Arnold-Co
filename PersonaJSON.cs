using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arnold_Co
{
    public class PersonaJSON
    {
        public string name;
        public string voiceID;
        public bool talksFast;

        public static void WriteTestJSON()
        {
            var test = new PersonaJSON();
            test.name = "Arnold";
            test.talksFast = false;
            test.voiceID = "am_adam";

            var serial = JsonConvert.SerializeObject(test, Program.serializerSettings);
            File.WriteAllText("Persona_Test.json", serial);
        }
    }
}
