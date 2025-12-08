using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Arnold_Co
{
    internal class DebugAction : ActionJSON
    {
        public override void OnCalled(string text)
        {
            base.OnCalled(text);
            int deviceCount = WaveIn.DeviceCount;
            for (int i = 0; i < deviceCount; i++)
            {
                var capabilities = WaveIn.GetCapabilities(i);
                Debug.WriteLine($"{i}: {capabilities.ProductName}");
            }
        }
    }
}
