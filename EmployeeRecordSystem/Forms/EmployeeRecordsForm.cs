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
            var result = this.openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ////string fileName = this.openFileDialog.FileName;
                ////MessageBox.Show(fileName);
            }
        }
    }
}