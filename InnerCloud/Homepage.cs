using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Speech.Recognition;

namespace InnerCloud
{
    public partial class Homepage : Form
    {
        SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        private int pathCount=22;
        private int count = 1;
        internal string cloudPath = @"H:\InnerCloudDB\";
        internal string filePath = "";
        internal List<string> listFiles = new List<string>();
        internal List<string> listDirec = new List<string>();
        public Homepage()
        {
            InitializeComponent();
        }

        private void Homepage_Load(object sender, EventArgs e)
        {
            txtPath.Text = @"C:\Users\Jami\Desktop";
            LoadFileList(txtPath.Text);
            btnCut.Enabled = false;
            btnCopy.Enabled = false;
            btnRename.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnRnSave.Visible = false;
            txtFileName.Visible = false;
            txtFolderName.Visible = false;
            btnNwFolder.Visible = false;
            btnDelFolder.Visible = false;
            btnRenameFolder.Visible = false;
            btnRnnFolder.Visible = false;

            Choices commands = new Choices();
            commands.Add(new string[] { " open", "copy", "cut", "rename", "folder rename done", "file rename done", "new folder", "folder done", "new file", "file done", "file delete", "home", "folder delete" });
            commands.Add("white");
            commands.Add("gray");
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(commands);
            Grammar g = new Grammar(gb);

            sre.LoadGrammarAsync(g);
            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += Sre_SpeechRecognized;
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "white":
                    this.BackColor = Color.White;
                    break;
                case "gray":
                    this.BackColor = Color.Gray;
                    break;
                case "open":
                    Process.Start(listFiles[listView.FocusedItem.Index]);
                    break;
                case "new folder":
                    ClickNewFolder();
                    break;
                case "new file":
                    CreateFile();
                    break;
                case "folder done":
                    NewFolderAction();
                    break;
                case "file done":
                    break;
                case "file delete":
                    DeleteFile();
                    break;
                case "folder delete":
                    DeleteFolder();
                    break;
                case "rename":
                    RenameFile();
                    break;
                case "file rename done":
                    RenameAction();
                    break;
                case "folder rename done":
                    RenameFolder();
                    break;
                case "copy":
                    CopyAction();
                    break;
                case "cut":
                    CutAction();
                    break;
                case "home":
                    GoToHome();
                    break;
            }
        }

        internal void LoadFileList(string path)
        {
            listFiles.Clear();
            listView.Items.Clear();
            listViewFolder.Items.Clear();

            foreach (string item in Directory.GetFiles(path))
            {
                FileInfo iArray = new FileInfo(item);
                listFiles.Add(iArray.FullName);
                imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                listView.Items.Add(iArray.Name, imageList.Images.Count - 1);
            }
            foreach (string dir in Directory.GetDirectories(path))
            {
                DirectoryInfo fArray = new DirectoryInfo(dir);
                listDirec.Add(fArray.FullName);
                listViewFolder.Items.Add(fArray.Name);
            }
            txtPath.Text = path;
        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.FocusedItem != null)
            {
                filePath = txtPath.Text + "\\" + listView.FocusedItem.Text;
                txtFileName.Text = listView.FocusedItem.Text;
            }

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            fbd.Description = "Select A Folder";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string path = fbd.SelectedPath;
                LoadFileList(path);
                txtPath.Text = path;
            }
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            btnCut.Enabled = true;
            btnCopy.Enabled = true;
            btnRename.Enabled = true;
            btnDelete.Visible = true;
            btnDelete.Enabled = true;
            btnDelFolder.Visible = false;
            btnRenameFolder.Visible = false;
            listViewSide.Items.Clear();
            btnRnnFolder.Visible = false;
            foreach (string item in Directory.GetFiles(txtPath.Text))
            {
                FileInfo iArray = new FileInfo(item);
                if (iArray.Name == listView.SelectedItems[0].Text)
                {
                    listViewSide.Items.Add("Name        : " + iArray.Name);
                    listViewSide.Items.Add("Size           : " + iArray.Length + "KB");
                    listViewSide.Items.Add("Created At : " + iArray.CreationTime);
                    listViewSide.Items.Add("File Type   : " + iArray.Extension);
                    listViewSide.Items.Add("Seen At     : " + iArray.LastAccessTime);
                    listViewSide.Items.Add("Modified At: " + iArray.LastWriteTime);
                    listViewSide.Items.Add("File Path    : " + iArray.Directory);
                    if (iArray.Extension == ".txt")
                    {
                        btnEdit.Enabled = true;
                    }
                    else if (iArray.Extension == ".docx")
                    {
                        btnEdit.Enabled = true;
                    }
                    else
                    {
                        btnEdit.Enabled = false;
                    }
                }
            }
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.FocusedItem != null)
            {
                Process.Start(listFiles[listView.FocusedItem.Index]);
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            GoToHome();
        }
        private void GoToHome()
        {
            txtPath.Text = @"C:\Users\Jami\Desktop";
            LoadFileList(txtPath.Text);
        }

        private void listViewFolder_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFolder.FocusedItem != null)
            {
                string fwdPath = txtPath.Text + "\\" + listViewFolder.FocusedItem.Text;
                LoadFileList(fwdPath);
                txtPath.Text = fwdPath;
            }
        }

        private void listViewFolder_MouseClick(object sender, MouseEventArgs e)
        {
            btnCut.Enabled = false;
            btnCopy.Enabled = false;
            btnRename.Enabled = false;
            btnDelete.Visible = false;
            btnDelFolder.Visible = true;
            btnRnnFolder.Visible = true;
            //btnRenameFolder.Visible = true;
            //txtFileName.Visible = true;
        }

        private void btnRnnFolder_Click(object sender, EventArgs e)
        {
            RenameFolderClick();
        }
        private void RenameFolderClick()
        {
            btnRnnFolder.Visible = false;
            btnRename.Visible = false;
            txtFileName.Visible = true;
            btnRenameFolder.Visible = true;
            btnRnSave.Visible = false;
            try
            {
                txtFileName.Text = listViewFolder.FocusedItem.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Folder is selected");
            }
        }
    }
}
