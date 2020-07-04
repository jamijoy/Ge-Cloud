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
    partial class Homepage : Form
    {
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteFile();
        }

        private void DeleteFile()
        {
            try
            {
                string cldPath = cloudPath + filePath.Substring(pathCount);
                string temp = @"H:\InnerCloudDB\trash\" + listView.FocusedItem.Text;

                File.Copy(filePath, temp);
                File.Delete(cldPath);
                File.Delete(filePath);
                MessageBox.Show("File Deleted !");
                LoadFileList(txtPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("no file selected");
            }
        }
        private void btnRename_Click(object sender, EventArgs e)
        {
            RenameFile();
        }

        private void RenameFile()
        {
            btnRename.Visible = false;
            txtFileName.Visible = true;
            btnRnSave.Visible = true;
            try
            {
                txtFileName.Text = listView.FocusedItem.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No file selected");
            }
        }
        private void btnRnSave_Click(object sender, EventArgs e)
        {
            RenameAction();
        }

        private void RenameAction()
        {
            txtFileName.Visible = false;
            btnRnSave.Visible = false;
            btnRename.Visible = true;
            try
            {
                if (File.Exists(filePath))
                {
                    string temp1 = cloudPath + filePath.Substring(pathCount);
                    string temp2 = filePath;
                    string cldPath = cloudPath + txtPath.Text.Substring(pathCount) + "\\" + txtFileName.Text;
                    string movePath = txtPath.Text + "\\" + txtFileName.Text;

                    File.Move(temp1, cldPath);
                    File.Move(temp2, movePath);
                    LoadFileList(txtPath.Text);
                    MessageBox.Show("Rename done");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not rename");
            }
        }
        private void btnCut_Click(object sender, EventArgs e)
        {
            CutAction();
        }
        private void CutAction()
        {
            try
            {
                FolderBrowserDialog fdd = new FolderBrowserDialog();
                fdd.Description = "Select a Folder to Move the File";
                fdd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
                fdd.ShowNewFolderButton = false;
                if (fdd.ShowDialog() == DialogResult.OK)
                {
                    string temp1 = cloudPath + filePath.Substring(pathCount);
                    string cldpath = cloudPath + fdd.SelectedPath.Substring(pathCount) + "\\" + listView.FocusedItem.Text;

                    string movePath = fdd.SelectedPath + "\\" + listView.FocusedItem.Text;

                    File.Move(filePath, movePath);
                    File.Move(temp1, cldpath);
                    LoadFileList(txtPath.Text);
                    MessageBox.Show("File MOVED", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("can not copy");
            }
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            CopyAction();
        }
        private void CopyAction()
        {
            FolderBrowserDialog fdd = new FolderBrowserDialog();
            fdd.Description = "Select a Folder to PASTE";
            fdd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            fdd.ShowNewFolderButton = false;

            try
            {
                if (fdd.ShowDialog() == DialogResult.OK)
                {
                    string copyPath = fdd.SelectedPath + "\\" + listView.FocusedItem.Text;

                    if (copyPath == filePath)
                    {
                        MessageBox.Show("same location", " Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string cloudSource = cloudPath + filePath.Substring(pathCount);
                        string cloudDestination = cloudPath + copyPath.Substring(pathCount);
                        File.Copy(filePath, copyPath);
                        File.Copy(cloudSource, cloudDestination);
                        LoadFileList(txtPath.Text);
                        MessageBox.Show("File COPIED", " Success", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Folder copy destination not selected !!", "Error !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("can not copy");
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            ClickNewFolder();
        }
        public void ClickNewFolder()
        {
            btnCreateFile.Visible = false;
            btnNewFolder.Visible = false;
            btnCut.Enabled = false;
            btnCopy.Enabled = false;
            btnRename.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnRnSave.Visible = false;
            txtFileName.Visible = false;
            txtFolderName.Visible = true;
            btnNwFolder.Visible = true;
        }
        private void btnNwFolder_Click(object sender, EventArgs e)
        {
            NewFolderAction();
        }
        public void NewFolderAction()
        {
            btnCreateFile.Visible = true;
            btnNewFolder.Visible = true;
            txtFolderName.Visible = false;
            btnNwFolder.Visible = false;
            try
            {
                if (txtFolderName.Text != "")
                {
                    string newDir = txtPath.Text + "\\" + txtFolderName.Text;
                    if(!Directory.Exists(newDir))
                    {
                        Directory.CreateDirectory(newDir);
                        Directory.CreateDirectory(cloudPath + newDir.Substring(pathCount));
                        LoadFileList(txtPath.Text);
                        MessageBox.Show("A New Folder Created");
                        txtFolderName.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("folder already exist.","Error");
                        txtFolderName.Text = "";
                    }
                }
                else
                {
                    LoadFileList(txtPath.Text);
                    MessageBox.Show("No Folder Created", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folder can't create");
            }
        }
        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            CreateFile();
        }

        private void CreateFile()
        {
            string dir = txtPath.Text;
            File_Manager fmc = new File_Manager(dir);
            fmc.Visible = true;
            fmc.btnUpdate.Visible = false;
            this.Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string filePath = txtPath.Text + "\\" + listView.FocusedItem.Text;
            //MessageBox.Show(filePath);
            File_Manager fm = new File_Manager(filePath);
            fm.Visible = true;
            fm.btnSave.Visible = false;
            this.Visible = false;
        }



        private void btnSpeech_Click(object sender, EventArgs e)
        {
            if (count == 1)
            {
                sre.RecognizeAsync(RecognizeMode.Multiple);
                this.btnSpeech.BackColor = Color.Green;
                count = 0;
            }
            else if (count == 0)
            {
                sre.RecognizeAsyncStop();
                this.btnSpeech.BackColor = Color.Red;
                count = 1;
            }
        }

        private void Homepage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void btnDelFolder_Click(object sender, EventArgs e)
        {
            DeleteFolder();
        }
        private void DeleteFolder()
        {
            if (listViewFolder.FocusedItem != null)
            {
                string dltFolderPath = txtPath.Text + "\\" + listViewFolder.FocusedItem.Text;
                string cldDltFolderPath = cloudPath + dltFolderPath.Substring(pathCount);
                Directory.Delete(dltFolderPath);
                Directory.Delete(cldDltFolderPath);
                LoadFileList(txtPath.Text);
                MessageBox.Show("Folder Has Been Deleted");
                btnDelFolder.Visible = false;
                btnDelete.Visible = true;
            }
        }

        private void btnRenameFolder_Click(object sender, EventArgs e)
        {
            RenameFolder();
        }
        private void RenameFolder()
        {
            txtFileName.Visible = false;
            btnRnSave.Visible = false;
            btnRenameFolder.Visible = false;
            btnRename.Visible = true;
            try
            {
                string temp2 = txtPath.Text + "\\" + listViewFolder.FocusedItem.Text;
                string newPath = txtPath.Text + "\\" + txtFileName.Text;
                string temp1 = cloudPath + temp2.Substring(pathCount);
                string cldPath = cloudPath + newPath.Substring(pathCount);

                Directory.Move(temp2, newPath);
                Directory.Move(temp1, cldPath);

                LoadFileList(txtPath.Text);
                MessageBox.Show("Rename done");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't be Renamed");
            }
        }
    }
}
