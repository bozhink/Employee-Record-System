namespace EmployeeRecordSystem.Forms
{
    using System;
    using System.Windows.Forms;
    using System.Xml;

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
                this.PopulateTreeView(this.openFileDialog.FileName);
                ////string fileName = this.openFileDialog.FileName;
                ////MessageBox.Show(fileName);
            }

            Settings.Default.InitialOpenDirectory = this.openFileDialog.InitialDirectory;
            Settings.Default.Save();
        }

        private void PopulateTreeView(string fileName)
        {
            this.toolStripStatusLabel.Text = "Refreshing employee Codes. Please wait...";
            this.Cursor = Cursors.WaitCursor;

            this.treeView.Nodes.Clear();
            var treeViewRootNode = new TreeNode("Employee Records");
            this.treeView.Nodes.Add(treeViewRootNode);

            TreeNodeCollection nodeCollection = treeViewRootNode.Nodes;
            string stringValue = string.Empty;

            var reader = XmlReader.Create(fileName);
            reader.MoveToElement();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasAttributes && reader.NodeType == XmlNodeType.Element)
                    {
                        reader.MoveToElement();
                        reader.MoveToElement();
                        reader.MoveToAttribute("id");
                        stringValue = reader.Value;
                        reader.Read();
                        reader.Read();

                        if (reader.Name == "Dept")
                        {
                            reader.Read();
                        }

                        TreeNode codeNode = new TreeNode(stringValue);
                        nodeCollection.Add(codeNode);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            this.Cursor = Cursors.Default;

            this.toolStripStatusLabel.Text = "Click on an employee record to see their record.";
        }
    }
}