using KokoroSharp;
using KokoroSharp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arnold_Co
{
    public class Persona
    {
        public string name;
        public string voiceID;

        public KokoroTTS speechSynth;
        public KokoroVoice voice;
        public void Init()
        {
            SetUpVoice();
        }

        public void Speak(string text)
        {
            if (speechSynth == null || voice == null) return;
            speechSynth.Speak(text, voice);
        }
        public void SetUpVoice()
        {
            voice = KokoroVoiceManager.GetVoice(voiceID);
            speechSynth = KokoroTTS.LoadModel();
            speechSynth.Speak("I am too aware of my existence.", voice);
        }
        public Persona(PersonaJSON json)
        {
            this.name = json.name;
            this.voiceID = json.voiceID;
            Init();
        }
    }
}
