using AudioSwitcher.AudioApi.CoreAudio;
using Microsoft.Win32.TaskScheduler;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using TTask = System.Threading.Tasks.Task;

namespace Arnold_Co
{
    internal class RoutineAction : ActionJSON
    {

        private void SetAlarm(int hour, int minute)
        {
            TimeSpan alarmTime = new TimeSpan(hour, minute, 0);
            DateTime now = DateTime.Now;

            DateTime today = now.Date + alarmTime;

            DateTime nextAlarm = (today > now)
                ? today
                : today.AddDays(1);

            TimeSpan waitTime = nextAlarm - now;
            Debug.WriteLine("Alarm will ring at: " + nextAlarm);

            string taskName = $"ArnoldAlarm_{hour:D2}{minute:D2}";


            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = $"Arnold Alarm: {hour:D2}:{minute:D2}";

                td.Triggers.Add(new TimeTrigger { StartBoundary = nextAlarm });

                td.Actions.Add(new ExecAction(
                    @"C:\Users\adamk\source\repos\Arnold&Co\bin\Debug\net10.0-windows\Arnold&Co.exe",
                    $"--alarm {hour:D2}:{minute:D2}",
                    @"C:\Users\adamk\source\repos\Arnold&Co\bin\Debug\net10.0-windows\"));

                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }
        public override void OnCalled(string text, Dictionary<string, object> parsedParams)
        {
            base.OnCalled(text, parsedParams);
            if(parsedParams.Count > 0) Debug.WriteLine(parsedParams.ElementAt(0).Value);
            SetShitUpDawg();   
        }


        public async TTask SetShitUpDawg()
        {
            ClearAllAlarms();

            SetAlarm(8, 00);
            SetAlarm(8, 10);
            OutputAction.ChangeOutput(OutputAction.speakerID);
        
            await TTask.Delay(5000);
            Debug.WriteLine("God hates me");

            CoreAudioDevice defaultDevice = new CoreAudioController().DefaultPlaybackDevice;
            Debug.WriteLine("current volume: " + defaultDevice.Volume);
            defaultDevice.Volume = 77;

        }
        private void SetVolume(float volume)
        {
            var deviceEnum = new MMDeviceEnumerator();  
            var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (volume == 0)
            {
                device.AudioEndpointVolume.Mute = true;
            }
            else
            {
                device.AudioEndpointVolume.Mute = false;
            }
            device.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
        }
        internal static void ClearAllAlarms()
        {
            using (TaskService ts = new TaskService())
            {
                var tasks = ts.RootFolder.Tasks;

                foreach (var t in tasks)
                {
                    if (t.Name.StartsWith("ArnoldAlarm_"))
                    {
                        Debug.WriteLine("Deleting task : " + t.Name);
                        ts.RootFolder.DeleteTask(t.Name, false);
                    }
                }
            }

            Debug.WriteLine("All Arnold alarms cleared.");
        }
    }
}
