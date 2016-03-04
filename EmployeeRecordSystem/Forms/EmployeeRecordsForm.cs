namespace EmployeeRecordSystem.Forms
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Ninject;

    using Services.Contracts;
    using Services.Models.Xml;

    public partial class EmployeeRecordsForm : Form
    {
        private string fileName;
        private DataRecords records;

        public EmployeeRecordsForm()
        {
            this.InitializeComponent();

            this.fileName = null;
            this.records = null;
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
                this.fileName = this.openFileDialog.FileName;
                this.PopulateTreeView().GetAwaiter();
            }

            Settings.Default.InitialOpenDirectory = this.openFileDialog.InitialDirectory;
            Settings.Default.Save();
        }

        private async Task PopulateTreeView()
        {
            if (!File.Exists(this.fileName))
            {
                MessageBox.Show(Messages.FileDoesNotExistMessage);
                return;
            }

            this.toolStripStatusLabel.Text = Messages.RefreshingEmployeeCodesMessage;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.treeView.Nodes.Clear();

                var service = this.kernel.Get<IEmployeeSerializationService>();

                this.records = null;
                using (var stream = new FileStream(this.fileName, FileMode.Open))
                {
                    this.records = await service.ReadEmployeeDataRecords(stream);
                }

                if (this.records == null)
                {
                    throw new ApplicationException(Messages.CannotReadEmployeeDataExceptionMessage);
                }

                var treeViewRootNode = new TreeNode(Settings.Default.TreeViewRootNodeName);
                var nodeCollection = treeViewRootNode.Nodes;

                foreach (var code in this.records.Codes)
                {
                    var codeNode = new TreeNode(code.Id);
                    nodeCollection.Add(codeNode);
                }

                this.treeView.Nodes.Add(treeViewRootNode);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            this.Cursor = Cursors.Default;
            this.toolStripStatusLabel.Text = Messages.SeeEmployeeRecordDataMessage;
        }

        private void PopulateListView(TreeNode treeNode)
        {
            this.InitializeListView();

            if (!File.Exists(this.fileName))
            {
                MessageBox.Show(Messages.FileDoesNotExistMessage);
                return;
            }

            try
            {
                string id = treeNode.Text;

                var matchingCodes = this.records.Codes.Where(i => i.Id == id);

                foreach (var code in matchingCodes)
                {
                    ListViewItem listViewItem = this.listView.Items.Add(code.EmployeeName);
                    listViewItem.SubItems.Add(code.Details.DateOfJoin.ToShortDateString());
                    listViewItem.SubItems.Add(code.Details.Grade.ToString());
                    listViewItem.SubItems.Add(code.Details.Salary.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void InitializeListView()
        {
            this.listView.Clear();

            this.listView.Columns.Add(
                Settings.Default.EmployeeNameColumnName, 
                Settings.Default.EmployeeNameColumnWidth,
                Settings.Default.EmployeeNameColumnHorizontalAlignment);

            this.listView.Columns.Add(
                Settings.Default.DateOfJoinColumnName,
                Settings.Default.DateOfJoinColumnWidth,
                Settings.Default.DateOfJoinColumnHorizontalAlignment);

            this.listView.Columns.Add(
                Settings.Default.GradeColumnName,
                Settings.Default.GradeColumnWidth,
                Settings.Default.GradeColumnHorizontalAlignment);

            this.listView.Columns.Add(
                Settings.Default.SalaryColumnName,
                Settings.Default.SalaryColumnWidth,
                Settings.Default.SalaryColumnHorizontalAlignment);
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currentNode = e.Node;
            if (this.treeView.TopNode == currentNode)
            {
                this.InitializeListView();
                this.toolStripStatusLabel.Text = Messages.DoubleClickEmployeeRecordsMessage;
                return;
            }
            else
            {
                this.toolStripStatusLabel.Text = Messages.SeeEmployeeRecordDataMessage;
            }

            this.PopulateListView(currentNode);
        }
    }
}