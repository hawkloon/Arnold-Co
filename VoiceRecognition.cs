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
        private WaveInEvent waveIn;

        public void Init()
        {
            Vosk.Vosk.SetLogLevel(0);

            Model model = new Model("models/vosk-model-small-en-us-0.15");
            recognizer = new VoskRecognizer(model, 16000.0f);

            recognizer.SetMaxAlternatives(0);
            recognizer.SetWords(true);
            recognizer.SetPartialWords(true);

            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 16, 1)
            };

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            Debug.WriteLine("Speech Recog Model loaded");
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
                    Wake();
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


        public void Wake()
        {
            state = ListeningState.Listening;
            Program.activePersona.Speak("What the fuck do you want");
            System.Media.SystemSounds.Hand.Play();
            wakeTime = DateTime.Now;
        }
        private bool FuzzyMatch(string text, string command)
        {
            text = text.ToLower();
            command = command.ToLower();

            if (text.Contains(command)) return true;

            return text.Similarity(command) >= 0.75f;
        }
        private Dictionary<string, object> ParseParameters(
    string text,
    ActionJSON action)
        {
            var result = new Dictionary<string, object>();
            var words = text.ToLower().Split(' ');

            if (action.parameters == null)
                return result;

            for (int i = 0; i < words.Length; i++)
            {
                foreach (var param in action.parameters)
                {
                    if (words[i] == param.name && i + 1 < words.Length)
                    {
                        string valueWord = words[i + 1];

                        object value = param.type switch
                        {
                            "int" => int.TryParse(valueWord, out var n) ? n : null,
                            "string" => valueWord,
                            _ => null
                        };

                        if (value != null)
                            result[param.name] = value;
                    }
                }
            }

            return result;
        }

        private void HandleCommand(string text)
        {
            Debug.WriteLine(text);
            foreach (var action in ActionManager.actions)
            {
                foreach (var keyword in action.keywords)
                {
                    if (FuzzyMatch(text, keyword))
                    {
                        Debug.WriteLine($"Action matched: {action.name}");

                        var parameters = ParseParameters(text, action);

                        action.OnCalled(text, parameters);
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
