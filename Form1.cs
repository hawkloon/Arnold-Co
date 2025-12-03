namespace Arnold_Co
{
    public partial class Form1 : Form
    {
        private static Form1 instance;

        public static Form1 Instance => instance != null ? instance : null;
        public Form1()
        {
            InitializeComponent();
            if (instance != null) this.Close();
            instance = this;
        }

        private void speechTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Program.SpeechTest();
        }

        private void transcribeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VoiceRecognition.TranscriptionTest();
        }
    }
}
