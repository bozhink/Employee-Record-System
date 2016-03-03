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
    }
}