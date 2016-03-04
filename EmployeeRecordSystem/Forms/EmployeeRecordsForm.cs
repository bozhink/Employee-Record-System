namespace EmployeeRecordSystem.Forms
{
    using System;
    using System.Windows.Forms;

    public partial class EmployeeRecordsForm : Form
    {
        public EmployeeRecordsForm()
        {
            this.InitializeComponent();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.InitialOpenDirectory))
            {
                string currentDirectory = this.openFileDialog.InitialDirectory;

                try
                {
                    this.openFileDialog.InitialDirectory = Settings.Default.InitialOpenDirectory;
                    currentDirectory = Settings.Default.InitialOpenDirectory;
                }
                catch
                {
                    this.openFileDialog.InitialDirectory = currentDirectory;
                    Settings.Default.InitialOpenDirectory = currentDirectory;
                }
            }

            var result = this.openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ////string fileName = this.openFileDialog.FileName;
                ////MessageBox.Show(fileName);
            }

            Settings.Default.InitialOpenDirectory = this.openFileDialog.InitialDirectory;
            Settings.Default.Save();
        }
    }
}