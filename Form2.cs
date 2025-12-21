using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace Arnold_Co
{
    public partial class Form2 : Form
    {
        SoundPlayer player;
        private System.Windows.Forms.Timer shakeTimer;
        private int shakeCount = 0;
        private int shakeMax = 20; // number of shakes
        private int shakeAmount = 10; // pixels to move
        private Point originalLocation;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            player.Stop();
            MessageBox.Show("you are a fucking piece of shit cunt", "Kill yourself faggot",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Activate();
            this.BringToFront();

            player = new SoundPlayer(@"C:\Windows\Media\Alarm01.wav");
            player.PlayLooping();
            originalLocation = this.Location;

            shakeTimer = new System.Windows.Forms.Timer();
            shakeTimer.Interval = 50; // how fast the shake updates
            shakeTimer.Tick += ShakeTick;
            shakeTimer.Start();

        }

        private void ShakeTick(object sender, EventArgs e)
        {
            // Random direction shake
            int offsetX = (shakeCount % 2 == 0) ? shakeAmount : -shakeAmount;
            this.Location = new Point(originalLocation.X + offsetX, originalLocation.Y);

            shakeCount++;
        }
    }
}

