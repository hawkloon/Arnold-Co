using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Vosk;
namespace Arnold_Co
{
    public class VoskWord
    {
        public string word { get; set; }
        public float conf { get; set; }
    }

    public class VoskResult
    {
        public string text { get; set; }
        public List<VoskWord> result { get; set; }
    }
    public class VoiceRecognition
    {
        VoskRecognizer recognizer;

        private enum ListeningState
        {
            Idle,
            Listening
        }

        private ListeningState state = ListeningState.Idle;
        private DateTime wakeTime;
        public void Init()
        {
            Vosk.Vosk.SetLogLevel(0);
            Model model = new Model("models/vosk-model-small-en-us-0.15");
            Debug.WriteLine("Speech Recog Model loaded");
            recognizer = new VoskRecognizer(model, 16000.0f);
            recognizer.SetMaxAlternatives(0);
            recognizer.SetWords(true);
            recognizer.SetPartialWords(true);

            var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 16, 1)
            };
            waveIn.DataAvailable += WaveIn_DataAvailable;

            waveIn.StartRecording();
        }
        public  void SwitchInput()
        {

        }
        private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            if (!recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                return;
            string json = recognizer.Result();

            var final = JsonConvert.DeserializeObject<VoskResult>(json);

            if (final?.result == null || string.IsNullOrWhiteSpace(final.text))
                return;
            float conf = final.result.Average(w => w.conf);

            if (conf < 0.7f)
                return;

            string text = final.text.ToLower().Trim();

            if(state == ListeningState.Idle)
            {
                if (text.Contains(Program.activePersona.name.ToLower()))
                {
                    state = ListeningState.Listening;
                    Program.activePersona.Speak("What the fuck do you want");
                    System.Media.SystemSounds.Hand.Play();
                    wakeTime = DateTime.Now;
                    return;
                }
                return;
            }

            if(state == ListeningState.Listening)
            {
                if((DateTime.Now - wakeTime).TotalSeconds > 5)
                {
                    Debug.WriteLine("Wake timed out, idling...");
                    state = ListeningState.Idle;
                    return;
                }

                HandleCommand(text);

            }

        }
        private bool FuzzyMatch(string text, string command)
        {
            text = text.ToLower();
            command = command.ToLower();

            if (text.Contains(command)) return true;

            return text.Similarity(command) >= 0.75f;
        }

        private void HandleCommand(string text)
        {
            foreach(var action in ActionManager.actions)
            {
                foreach(var keyword in action.keywords)
                {
                    if(FuzzyMatch(text, keyword))
                    {
                        Debug.WriteLine($"Action matched: {action.name} for command: {text}");
                        action.OnCalled(text);
                        state = ListeningState.Idle;
                        return;
                    }
                }
            }
        }


        public static void TranscriptionTest()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Audio Files (*.mp3;*.wav)|*.mp3;*.wav";
                ofd.Title = "Select an audio file";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Pass file to your Vosk transcription
                    VoiceRecognition.TranscribeMP3(ofd.FileName);
                }
            }
        }
        public static void TranscribeMP3(string path)
        {
            var model = new Model("models/vosk-model-small-en-us-0.15");

            using var mp3 = new Mp3FileReader(path);

            var resample = new MediaFoundationResampler(mp3, new WaveFormat(16000, 1));

            resample.ResamplerQuality = 60;

            var recognizer = new VoskRecognizer(model, 16000.0f);
            recognizer.SetMaxAlternatives(0);
            recognizer.SetWords(true);

            byte[] buffer = new byte[4096];
            int bytesRead;

            while((bytesRead = resample.Read(buffer, 0, buffer.Length)) > 0)
            {
                if(recognizer.AcceptWaveform(buffer, bytesRead))
                {
                    Debug.WriteLine("FINAL " + recognizer.Result());
                }
            }

            Debug.WriteLine($"FINAL: {recognizer.FinalResult()}");
        }
    }
}
