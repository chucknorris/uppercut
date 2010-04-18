namespace uppercut.settings.editor
{
    using System;
    using System.Windows.Forms;

    public partial class SettingsEditor : Form
    {
        public SettingsEditor()
        {
            InitializeComponent();
        }

        private void SettingsEditor_Load(object sender, EventArgs e)
        {
        }

        private void settingsFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        { 
            ofdSettings.ShowDialog();
           
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadGridWithFile()
        {
        }
    }
}