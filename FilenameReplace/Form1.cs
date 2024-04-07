using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilenameReplace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = "C:\\Temp\\Manga";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = dlg.SelectedPath;
                DirectoryInfo dirInfo = new DirectoryInfo(txtFolder.Text);
                FileInfo [] aryFiles = dirInfo.GetFiles();
                StringBuilder sbText = new StringBuilder();
                foreach (FileInfo fileInfo in aryFiles)
                {
                    sbText.AppendLine(fileInfo.Name);
                }
                txtResult.Text = sbText.ToString();
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                Regex regex = new Regex(txtRegexFind.Text);
                DirectoryInfo dirInfo = new DirectoryInfo(txtFolder.Text);
                FileInfo[] aryFiles = dirInfo.GetFiles();
                StringBuilder sbText = new StringBuilder();
                foreach (FileInfo fileInfo in aryFiles)
                {
                    string sResult = regex.Replace(fileInfo.Name, txtRegexReplace.Text);
                    sbText.AppendLine($"{fileInfo.Name}\t\t=>  {sResult}");
                }
                txtResult.Text = sbText.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            try
            {
                Regex regex = new Regex(txtRegexFind.Text);
                DirectoryInfo dirInfo = new DirectoryInfo(txtFolder.Text);
                FileInfo[] aryFiles = dirInfo.GetFiles();
                StringBuilder sbText = new StringBuilder();
                foreach (FileInfo fileInfo in aryFiles)
                {
                    string sResult = regex.Replace(fileInfo.Name, txtRegexReplace.Text);
                    string sNewFilename = Path.Combine(fileInfo.Directory.FullName, sResult);

                    try
                    {
                        fileInfo.MoveTo(sNewFilename);
                        sbText.AppendLine($"{fileInfo.Name}\t\t=>  {sResult}");
                    }
                    catch(Exception ex) 
                    {
                        sbText.AppendLine($"{fileInfo.Name}\t\t =>  {ex.Message} !!!");
                    }
                    
                    
                }
                txtResult.Text = sbText.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
