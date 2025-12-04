using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Arnold_Co
{
    public class SwitchAction : ActionJSON
    {
        public override void OnCalled(string text)
        {
            base.OnCalled(text);
            Debug.WriteLine("Switching");
            Program.LoadPersona(Program.allPersonaIDs.PickRandom());
        }
    }
}
