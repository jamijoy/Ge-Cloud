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

namespace InnerCloud
{
    public partial class File_Manager : Form
    {
        private int pathCount = 22;
        string fileName, path, getLine = "", line;
        public File_Manager(string path)
        {
            InitializeComponent();
            this.path = path;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Homepage hm = new Homepage();
            hm.Visible = true;
            this.Visible = false;
        }

        private void File_Manager_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void File_Manager_Load(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        getLine = getLine + line + "\n";
                    }
                    txtData.Text = getLine;
                }
                FileInfo fi = new FileInfo(path);
                txtFileName.Text = fi.Name;
                txtFileName.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string dsPath = @"H:\InnerCloudDB\" + path.Substring(pathCount);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (File.Exists(dsPath))
                {
                    File.Delete(dsPath);
                }

                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(txtData.Text);
                }
                using (StreamWriter sw = File.CreateText(dsPath))
                {
                    sw.WriteLine(txtData.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("File Has Been Updated");
            Homepage hm = new Homepage();
            hm.Visible = true;
            //hm.LoadFileList(path);
            this.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text == "")
            {
                MessageBox.Show("File Name Field is Blank");
            }
            else if (txtFileName.Text != "")
            {
                fileName = txtFileName.Text + ".txt";
            }

            string str = @path + "\\" + fileName;

            try
            {
                if (File.Exists(str))
                {
                    MessageBox.Show("File Already Exists, Rename The File");
                }
                else
                {
                    string dsPath = @"H:\InnerCloudDB\" + str.Substring(pathCount);

                    using (StreamWriter sw = File.CreateText(str))
                    {
                        sw.WriteLine(txtData.Text);
                    }
                    using (StreamWriter sw = File.CreateText(dsPath))
                    {
                        sw.WriteLine(txtData.Text);
                    }
                    MessageBox.Show("File Has Been Created");

                    Homepage hm = new Homepage();
                    hm.Visible = true;
                    hm.LoadFileList(path);
                    this.Visible = false;
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
