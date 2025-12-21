namespace Arnold_Co
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            trayContextMenu = new ContextMenuStrip(components);
            settingsToolStripMenuItem = new ToolStripMenuItem();
            openPromptToolStripMenuItem = new ToolStripMenuItem();
            muteToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            speechTestToolStripMenuItem = new ToolStripMenuItem();
            transcribeToolStripMenuItem = new ToolStripMenuItem();
            clearAlarmsToolStripMenuItem = new ToolStripMenuItem();
            arnoldIcon = new NotifyIcon(components);
            trayContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // trayContextMenu
            // 
            trayContextMenu.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, openPromptToolStripMenuItem, muteToolStripMenuItem, speechTestToolStripMenuItem, transcribeToolStripMenuItem, clearAlarmsToolStripMenuItem, quitToolStripMenuItem });
            trayContextMenu.Name = "trayContextMenu";
            trayContextMenu.Size = new Size(181, 180);
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(180, 22);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // openPromptToolStripMenuItem
            // 
            openPromptToolStripMenuItem.Name = "openPromptToolStripMenuItem";
            openPromptToolStripMenuItem.Size = new Size(180, 22);
            openPromptToolStripMenuItem.Text = "Open Prompt";
            // 
            // muteToolStripMenuItem
            // 
            muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            muteToolStripMenuItem.Size = new Size(180, 22);
            muteToolStripMenuItem.Text = "Mute";
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(180, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // speechTestToolStripMenuItem
            // 
            speechTestToolStripMenuItem.Name = "speechTestToolStripMenuItem";
            speechTestToolStripMenuItem.Size = new Size(180, 22);
            speechTestToolStripMenuItem.Text = "Speech Test";
            speechTestToolStripMenuItem.Click += speechTestToolStripMenuItem_Click;
            // 
            // transcribeToolStripMenuItem
            // 
            transcribeToolStripMenuItem.Name = "transcribeToolStripMenuItem";
            transcribeToolStripMenuItem.Size = new Size(180, 22);
            transcribeToolStripMenuItem.Text = "Transcribe";
            transcribeToolStripMenuItem.Click += transcribeToolStripMenuItem_Click;
            // 
            // clearAlarmsToolStripMenuItem
            // 
            clearAlarmsToolStripMenuItem.Name = "clearAlarmsToolStripMenuItem";
            clearAlarmsToolStripMenuItem.Size = new Size(180, 22);
            clearAlarmsToolStripMenuItem.Text = "Clear Alarms";
            clearAlarmsToolStripMenuItem.Click += clearAlarmsToolStripMenuItem_Click;
            // 
            // arnoldIcon
            // 
            arnoldIcon.Icon = (Icon)resources.GetObject("arnoldIcon.Icon");
            arnoldIcon.Text = "ArnoldIcon";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ContextMenuStrip = trayContextMenu;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            trayContextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem openPromptToolStripMenuItem;
        private ToolStripMenuItem muteToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        internal NotifyIcon arnoldIcon;
        internal ContextMenuStrip trayContextMenu;
        private ToolStripMenuItem speechTestToolStripMenuItem;
        private ToolStripMenuItem transcribeToolStripMenuItem;
        private ToolStripMenuItem clearAlarmsToolStripMenuItem;
    }
}
