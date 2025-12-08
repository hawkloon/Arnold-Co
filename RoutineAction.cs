using Microsoft.Win32.TaskScheduler;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;

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
                td.RegistrationInfo.Description = "Arnold Alarm";

                td.Triggers.Add(new TimeTrigger { StartBoundary = nextAlarm });

                td.Actions.Add(new ExecAction(
                    @"C:\Users\adamk\source\repos\Arnold&Co\bin\Debug\net10.0-windows\Arnold&Co.exe",
                    $"--alarm {hour:D2}:{minute:D2}",
                    @"C:\Users\adamk\source\repos\Arnold&Co\bin\Debug\net10.0-windows\"));

                ts.RootFolder.RegisterTaskDefinition(@"ArnoldAlarm", td);
            }
        }
        public override void OnCalled(string text)
        {
            base.OnCalled(text);

            ClearAllAlarms();

            SetAlarm(4, 40);
            SetAlarm(4, 50);
            OutputAction.ChangeOutput(OutputAction.speakerID);
            var t = System.Threading.Tasks.Task.Run(async delegate
            {
                await System.Threading.Tasks.Task.Delay(1000);
                SetVolume(77);
            });
            t.Wait();
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
        private void ClearAllAlarms()
        {
            using (TaskService ts = new TaskService())
            {
                var tasks = ts.RootFolder.Tasks;

                foreach (var t in tasks)
                {
                    if (t.Name.StartsWith("ArnoldAlarm_"))
                    {
                        ts.RootFolder.DeleteTask(t.Name, false);
                    }
                }
            }

            Debug.WriteLine("All Arnold alarms cleared.");
        }
    }
}
