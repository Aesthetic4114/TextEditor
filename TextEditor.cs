using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

class TextEditorForm : Form
{
    private TextBox editorTextBox;
    private string currentFilePath;

    public TextEditorForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Set form properties
        Text = "Text Editor";
        Size = new Size(800, 600);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.LightGray;
        StartPosition = FormStartPosition.CenterScreen;

        // Create and configure controls
        editorTextBox = new TextBox();
        editorTextBox.Multiline = true;
        editorTextBox.ScrollBars = ScrollBars.Vertical;
        editorTextBox.Dock = DockStyle.Fill;
        editorTextBox.BackColor = Color.DarkGray;
        editorTextBox.ForeColor = Color.White;
        editorTextBox.Font = new Font("Arial", 12f);

        MenuStrip menuStrip = new MenuStrip();
        menuStrip.BackColor = Color.DarkGray;
        menuStrip.ForeColor = Color.White;

        ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
        ToolStripMenuItem newMenuItem = new ToolStripMenuItem("New");
        newMenuItem.Click += NewMenuItem_Click;
        ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Open");
        openMenuItem.Click += OpenMenuItem_Click;
        ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Save");
        saveMenuItem.Click += SaveMenuItem_Click;

        fileMenu.DropDownItems.Add(newMenuItem);
        fileMenu.DropDownItems.Add(openMenuItem);
        fileMenu.DropDownItems.Add(saveMenuItem);

        menuStrip.Items.Add(fileMenu);

        // Set controls on the form
        Controls.Add(editorTextBox);
        Controls.Add(menuStrip);
    }

    private void NewMenuItem_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(editorTextBox.Text))
        {
            DialogResult result = MessageBox.Show("Do you want to save the current file before creating a new one?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SaveFile();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        editorTextBox.Text = "";
        currentFilePath = null;
    }

    private void OpenMenuItem_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(editorTextBox.Text))
        {
            DialogResult result = MessageBox.Show("Do you want to save the current file before opening another file?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SaveFile();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string filePath = openFileDialog.FileName;

            try
            {
                editorTextBox.Text = File.ReadAllText(filePath);
                currentFilePath = filePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void SaveMenuItem_Click(object sender, EventArgs e)
    {
        SaveFile();
    }

    private void SaveFile()
    {
        if (string.IsNullOrEmpty(currentFilePath))
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllText(filePath, editorTextBox.Text);
                    currentFilePath = filePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            try
            {
                File.WriteAllText(currentFilePath, editorTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    [STAThread]
    static void Main(string[] args)
    {
        Application.Run(new TextEditorForm());
    }
}
