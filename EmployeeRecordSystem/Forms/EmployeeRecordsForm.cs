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

            try
            {
                var reader = XmlReader.Create(fileName);
                reader.MoveToElement();
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

        private void PopulateListView(string fileName, TreeNode treeNode)
        {
            this.InitializeListView();
            try
            {
                var reader = XmlReader.Create(fileName);
                reader.MoveToElement();
                while (reader.Read())
                {
                    string nodeName;
                    string nodePath;
                    string name;
                    string grade;
                    string dateOfJoin;
                    string salary;
                    string[] itemsArray = new string[4];

                    reader.MoveToFirstAttribute();
                    nodeName = reader.Value;
                    nodePath = treeNode.FullPath.Remove(0, 17);

                    if (nodePath == nodeName)
                    {
                        ListViewItem listViewItem;

                        reader.MoveToNextAttribute();
                        name = reader.Value;
                        listViewItem = this.listView.Items.Add(name);

                        reader.Read();
                        reader.Read();

                        reader.MoveToFirstAttribute();
                        dateOfJoin = reader.Value;
                        listViewItem.SubItems.Add(dateOfJoin);

                        reader.MoveToNextAttribute();
                        grade = reader.Value;
                        listViewItem.SubItems.Add(grade);

                        reader.MoveToNextAttribute();
                        salary = reader.Value;
                        listViewItem.SubItems.Add(salary);

                        reader.MoveToNextAttribute();
                        reader.MoveToElement();
                        reader.Read();
                    }
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
            this.listView.Columns.Add("Employee Name", 255, HorizontalAlignment.Left);
            this.listView.Columns.Add("Date of join", 70, HorizontalAlignment.Right);
            this.listView.Columns.Add("Grade", 105, HorizontalAlignment.Left);
            this.listView.Columns.Add("Salary", 105, HorizontalAlignment.Left);
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currentNode = e.Node;
            if (this.treeView.TopNode == currentNode)
            {
                this.InitializeListView();
                this.toolStripStatusLabel.Text = "Double click the Employee Records.";
                return;
            }
            else
            {
                this.toolStripStatusLabel.Text = "Click an employee code to vew individual records.";
            }

            this.PopulateListView(this.openFileDialog.FileName, currentNode);
        }
    }
}